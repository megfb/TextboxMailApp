using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TextboxMailApp.Application.Contracts.Persistence;

namespace TextboxMailApp.Persistence.Extensions
{
    public static class MailExtension
    {
        public static IServiceCollection AddMailKitService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailReader, MailKitEmailReader>();
            services.AddSingleton<IPersistentMailClient, PersistentMailClient>();
            return services;
        }
    }
}
