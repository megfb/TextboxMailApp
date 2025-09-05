using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TextboxMailApp.Api.Extensions
{
    public static class JwtService
    {
        public static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,

                      ValidIssuer = configuration["Jwt:Issuer"],
                      ValidAudience = configuration["Jwt:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                      )
                  };
              });
            services.AddAuthorization();

            return services;
        }
    }
}
