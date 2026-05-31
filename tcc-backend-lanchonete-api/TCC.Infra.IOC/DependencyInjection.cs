using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TCC.Application.Service.Interfaces;
using TCC.Application.Service.Services;
using TCC.Domain.Interfaces;
using TCC.Infra.Data.Context;
using TCC.Infra.Data.Repositories;
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

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioRoleRepository, UsuarioRoleRepository>();
            services.AddScoped<IUnidadeRepository, UnidadeRepository>();
            services.AddScoped<ICardapioRepository, CardapioRepository>();
            services.AddScoped<IEstoqueRepository, EstoqueRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUnidadeService, UnidadeService>();
            services.AddScoped<ICardapioService, CardapioService>();
            services.AddScoped<IPedidoService, PedidoService>();

            return services;
        }
    }
}
