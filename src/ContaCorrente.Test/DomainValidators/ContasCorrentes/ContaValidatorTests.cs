using ContaCorrente.Domain._Shared;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain.ContasCorrentes.Validators;
using FluentAssertions;
using Moq;

namespace ContaCorrente.Test.DomainValidators.ContasCorrentes
{
    public class ContaValidatorTests
    {
        private readonly Mock<IContaRepository> _repositoryMock;
        private readonly ContaValidator _validator;

        public ContaValidatorTests()
        {
            _repositoryMock = new Mock<IContaRepository>();
            _validator = new ContaValidator(_repositoryMock.Object);
        }

        [Fact]
        public void CreateValidator_ContaNaoExiste_DeveRetornarNull()
        {
            // Arrange
            _repositoryMock.Setup(r => r.ExisteContaPorCpf("12345678900", false)).Returns(false);

            // Act
            var result = _validator.CreateValidator("12345678900");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void CreateValidator_ContaJaExiste_DeveRetornarErro()
        {
            // Arrange
            _repositoryMock.Setup(r => r.ExisteContaPorCpf("12345678900", false)).Returns(true);

            // Act
            var result = _validator.CreateValidator("12345678900");

            // Assert
            result.ErrorType.Should().Be("INVALID_DOCUMENT");
            result.Message.Should().Be("CPF já possui uma conta.");            
        }
    }
}
