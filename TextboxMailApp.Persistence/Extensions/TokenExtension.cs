using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Persistence.Security;

namespace TextboxMailApp.Persistence.Extensions
{
    public static class TokenExtension
    {
        public static IServiceCollection AddTokenService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}
