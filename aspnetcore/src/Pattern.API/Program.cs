using System.Globalization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pattern.API.Extensions;
using Pattern.API.Filters;
using Pattern.API.Middlewares;
using Pattern.Application.Services.Base;
using Pattern.Application.Services.Emails;
using Pattern.Application.Services.Users.Mapper;
using Pattern.Application.Services.Users.Validators;
using Pattern.Core.Authentication;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Entites.BaseEntity;
using Pattern.Core.Localization;
using Pattern.Core.Options;
using Pattern.Persistence.Context;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Localization;
using Pattern.Core;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => { options.Filters.Add<ValidateFilterAttribute>(); });

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<CreateUserValidator>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Pattern", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

builder.Services.AddAutoMapper(typeof(UserMapper));
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

builder.Services.AddIdentity<User, Role>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<LocalizationIdentityErrorDescriber>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
    opts.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = tokenOptions!.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = tokenOptions.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

// Microsoft Sql Server
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//	options
//	.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
//	.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
//});


// MySql
// string mySqlConnectionStr = builder.Configuration.GetConnectionString("Default")!;
// builder.Services.AddDbContext<ApplicationDbContext>(u => u
//     .UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr),
//         options => options.EnableRetryOnFailure(
//             maxRetryCount: 5,
//             maxRetryDelay: TimeSpan.FromSeconds(30),
//             errorNumbersToAdd: null
//         ))
//     .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
// );

// PostgreSql
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("Default"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.Configure<FrontInformation>(builder.Configuration.GetSection("FrontInformation"));
builder.Services.AddRepositoriesForEntities(typeof(Entity).Assembly);
builder.Services.AddServices(typeof(ApplicationService).Assembly);


// Cors
builder.Services
    .AddCors(options =>
        options.AddPolicy("MyPolicy", policyBuilder =>
            policyBuilder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
        ));


// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        if (context.User.Identity is { IsAuthenticated: true })
        {
            var userId = context.User.Identity.Name ?? "unknownUser";
            return RateLimitPartition.GetFixedWindowLimiter(userId, _ =>
                new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 3
                });
        }

        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknownIP";
        return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = 50,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 3
            });
    });

    options.RejectionStatusCode = 429;
});


// Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.PostgreSQL(
        connectionString: builder.Configuration.GetConnectionString("Default"),
        tableName: "logs",
        needAutoCreateTable: true)
    .MinimumLevel.Warning()
    .CreateLogger();

builder.Host.UseSerilog();

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddScoped<IResourceLocalizer, ResourceLocalizer>();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("tr-TR")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseCors("MyPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();