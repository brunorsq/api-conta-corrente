using ContaCorrente.Domain.ContasCorrentes.Entities;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain.ContasCorrentes.Validators;
using ContaCorrente.Domain._Shared;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Runtime.CompilerServices;

namespace ContaCorrente.Test.Helpers
{
    public class CriarContaHelper
    {
        private readonly Mock<IContaRepository> _mockRepository;
        public CriarContaHelper() 
        {
            _mockRepository = new Mock<IContaRepository>();

            var services = new ServiceCollection();

            services.AddSingleton(_mockRepository.Object);
            services.AddScoped<ContaValidator>();

            var serviceProvider = services.BuildServiceProvider();

            ValidatorProvider.Initialize(serviceProvider);
        }
        public async Task<ContaCorrenteEntity> CriarContaParaTeste(string id = "12345678900", long numero = 100001, string nome = "João", string senha = "senha")
        {
            return await new ContaCorrenteEntity().Create(id, numero, nome, senha);
        }

        public ContaCorrenteEntity InativarContaTeste(ContaCorrenteEntity conta)
        {
            return new ContaCorrenteEntity().Inative(conta);
        }
    }
}
