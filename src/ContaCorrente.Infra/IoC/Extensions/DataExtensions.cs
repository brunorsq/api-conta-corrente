using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain.Movimentos.Repository;
using ContaCorrente.Infra.Data.Context;
using ContaCorrente.Infra.Data.Repository.ContaCorrente;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContaCorrente.Infra.IoC.Extensions
{
    internal static class DataExtensions
    {
        internal static IServiceCollection AdicionarDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Dapper
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
            var dbPath = Path.Combine(projectRoot, "database", "contacorrente.db");

            var connectionString = configuration
                .GetConnectionString("DefaultConnection")
                .Replace("{DB_PATH}", dbPath);

            services.AddSingleton<DapperContext>(new DapperContext(connectionString));

            // Repositórios
            services.AddScoped<IContaRepository, ContaRepository>();
            services.AddScoped<IMovimentoRepository, MovimentoRepository>();

            return services;
        }
    }
}
