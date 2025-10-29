using ContaCorrente.Domain._Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaCorrente.Infra.IoC.Extensions
{
    internal static class ProviderExtensions
    {
        internal static IServiceCollection AdicionarProvider(this IServiceCollection services)
        {
            ValidatorProvider
                .Initialize(services.BuildServiceProvider());

            return services;
        }
    }
}

