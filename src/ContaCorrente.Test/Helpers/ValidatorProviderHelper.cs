using ContaCorrente.Domain._Shared;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain.ContasCorrentes.Validators;
using ContaCorrente.Domain.Movimentos.Validators;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ContaCorrente.Test.Helpers
{
    internal class ValidatorProviderHelper
    {
        public static void InitializeWithMocks(Mock<IContaRepository> contaRepoMock)
        {
            typeof(ValidatorProvider)
                .GetField("_instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, null);

            var services = new ServiceCollection();
            services.AddSingleton(contaRepoMock.Object);
            services.AddScoped<MovimentoValidator>();
            services.AddScoped<ContaValidator>();

            ValidatorProvider.Initialize(services.BuildServiceProvider());
        }
    }
}
