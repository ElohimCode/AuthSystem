using Application.Contracts.Services.Identity;
using Infrastructure.Services.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services
                .AddTransient<ITokenService, TokenService>();

            return services;
        }
    }
}
