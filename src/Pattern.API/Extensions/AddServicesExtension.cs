using Pattern.Application.Services.Authentication;
using Pattern.Application.Services.Emails;
using Pattern.Application.Services.Users;
using Pattern.Persistence.UnitOfWork;

namespace Pattern.API.Extensions
{
    public static class AddServicesExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            // It's manual for now but I will make it automatic in the future. It's just service classes not repositories class.
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}
