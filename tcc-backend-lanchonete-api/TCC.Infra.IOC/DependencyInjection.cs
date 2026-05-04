using Microsoft.Extensions.DependencyInjection;
using TCC.Application.Service.Interfaces;
using TCC.Application.Service.Services;
using TCC.Infra.Security.Interfaces;
using TCC.Infra.Security.Token;

namespace TCC.Infra.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILoginService, LoginService>();
            return services;
        }
    }
}
