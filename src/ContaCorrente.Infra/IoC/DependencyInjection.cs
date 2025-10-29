using ContaCorrente.Infra.IoC.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContaCorrente.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration)
                .AdicionarDataContext(configuration)
                .AdicionarMediatR()
                .AdicionarAuthentication(configuration)
                .AdicionarSwagger()
                .AdicionarUtils()
                .AdicionarValidations()
                .AdicionarProvider();


            return services;
        }
    }
}
