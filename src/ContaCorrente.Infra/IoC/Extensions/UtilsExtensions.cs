using ContaCorrente.Application.Interfaces.Utils;
using ContaCorrente.Application.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ContaCorrente.Infra.IoC.Extensions
{
    internal static class UtilsExtensions
    {
        internal static IServiceCollection AdicionarUtils(this IServiceCollection services)
        {
            services.AddScoped<IPasswordValidator, PasswordValidator>();
            services.AddScoped<ITokenJWT, TokenJWT>();

            return services;
        }
    }
}
