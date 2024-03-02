using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pattern.API.Extensions;
using Pattern.API.Filters;
using Pattern.API.Middlewares;
using Pattern.Application.Services.Base;
using Pattern.Application.Services.Emails;
using Pattern.Application.Services.Users.Mapper;
using Pattern.Core.Authentication;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Entites.BaseEntity;
using Pattern.Core.Localization;
using Pattern.Core.Options;
using Pattern.Persistence.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
	options.Filters.Add<ValidateFilterAttribute>();
}).AddFluentValidation(options =>
{
	options.RegisterValidatorsFromAssemblyContaining<ApplicationService>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(UserMapper));
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true;
});

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
		ValidIssuer = tokenOptions.Issuer,
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

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//	options
//	.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
//	.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
//});

string mySqlConnectionStr = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddDbContext<ApplicationDbContext>(u => u
	.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr),
		options => options.EnableRetryOnFailure(
			maxRetryCount: 5,
			maxRetryDelay: TimeSpan.FromSeconds(30),
			errorNumbersToAdd: null
		))
	.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.Configure<FrontInformation>(builder.Configuration.GetSection("FrontInformation"));
builder.Services.AddRepositoriesForEntities(typeof(Entity).Assembly);
builder.Services.AddServices(typeof(ApplicationService).Assembly);

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
	builder.AllowAnyOrigin()
		   .AllowAnyMethod()
		   .AllowAnyHeader();
}));


var app = builder.Build();

app.UseCors(builder =>
		builder
		.AllowAnyOrigin()
		.AllowAnyMethod()
		.AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseGlobalExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
