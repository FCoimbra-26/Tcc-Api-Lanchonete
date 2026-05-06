using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using TCC.Application.Service.Interfaces;
using TCC.Application.Service.Services;
using TCC.Infra.Data.Context;
using TCC.Infra.Security.Interfaces;
using TCC.Infra.Security.Token;


namespace TCC.Infra.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TCC"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILoginService, LoginService>();
            return services;
        }
    }
}
