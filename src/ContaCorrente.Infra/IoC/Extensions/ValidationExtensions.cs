using ContaCorrente.Domain.ContasCorrentes.Validators;
using ContaCorrente.Domain.Movimentos.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaCorrente.Infra.IoC.Extensions
{
    internal static class ValidationExtensions
    {
        internal static IServiceCollection AdicionarValidations(this IServiceCollection services)
        {
            services.AddScoped<ContaValidator>();
            services.AddScoped<MovimentoValidator>();

            return services;
        }
    }
}
