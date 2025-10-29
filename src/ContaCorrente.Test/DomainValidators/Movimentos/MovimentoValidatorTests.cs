using ContaCorrente.Domain._Shared;
using ContaCorrente.Domain._Shared.Enums;
using ContaCorrente.Domain.ContasCorrentes.Entities;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain.ContasCorrentes.Validators;
using ContaCorrente.Domain.Movimentos.Validators;
using ContaCorrente.Test.Helpers;
using FluentAssertions;
using Moq;

namespace ContaCorrente.Test.DomainValidators.Movimentos
{
    public class MovimentoValidatorTests
    {
        private readonly Mock<IContaRepository> _repositoryMock;
        private readonly MovimentoValidator _validator;
        private readonly CriarContaHelper _criarContaHelper;
        public MovimentoValidatorTests()
        {
            _repositoryMock = new Mock<IContaRepository>();
            _validator = new MovimentoValidator(_repositoryMock.Object);
            _criarContaHelper = new CriarContaHelper();
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Quando_Conta_Nao_Existir()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.BuscarContaCorrenteAsync("12345678900"))
                .ReturnsAsync((ContaCorrenteEntity)null);

            // Act
            var result = await _validator.CreateValidator("12345678900");

            // Assert
            result.Should().NotBeNull();
            result.ErrorType.Should().Be("INVALID_ACCOUNT");
            result.Message.Should().Be("Conta não encontrada.");
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Quando_Conta_Estiver_Inativa()
        {
            // Arrange
            var conta = await _criarContaHelper.CriarContaParaTeste();
            var contaInativa = _criarContaHelper.InativarContaTeste(conta);

            _repositoryMock
                .Setup(r => r.BuscarContaCorrenteAsync("12345678900"))
                .ReturnsAsync(contaInativa);

            // Act
            var result = await _validator.CreateValidator("12345678900");

            // Assert
            result.Should().NotBeNull();
            result.ErrorType.Should().Be("INACTIVE_ACCOUNT");
            result.Message.Should().Be("Conta está inativa.");
        }

        [Fact]
        public async Task Deve_Retornar_Null_Quando_Conta_For_Valida()
        {
            // Arrange
            var conta = await _criarContaHelper.CriarContaParaTeste();
            
            _repositoryMock
                .Setup(r => r.BuscarContaCorrenteAsync("12345678900"))
                .ReturnsAsync(conta);

            // Act
            var result = await _validator.CreateValidator("12345678900");

            // Assert
            result.Should().BeNull();
        }
    }
}
