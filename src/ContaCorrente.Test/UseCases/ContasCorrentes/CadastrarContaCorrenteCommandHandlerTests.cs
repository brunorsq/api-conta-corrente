using ContaCorrente.Application.UseCases.ContaCorrente.CadastrarContaCorrente;
using ContaCorrente.Domain._Shared;
using ContaCorrente.Domain.ContasCorrentes.Entities;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Test.Helpers;
using FluentAssertions;
using Moq;


namespace ContaCorrente.Test.UseCases.ContasCorrentes
{
    public class CadastrarContaCorrenteCommandHandlerTests
    {
        private readonly Mock<IContaRepository> _mockRepository;

        public CadastrarContaCorrenteCommandHandlerTests()
        {
            _mockRepository = new Mock<IContaRepository>();

            ValidatorProviderHelper.InitializeWithMocks(_mockRepository);
        }

        [Fact]
        public async Task Handle_DeveCadastrarContaCorrenteComSucesso()
        {
            // Arrange
            var mockRepository = new Mock<IContaRepository>();

            mockRepository.Setup(r => r.ObterProximoNumeroAsync())
                .ReturnsAsync(12345);

            var handler = new CadastrarContaCorrenteCommandHandler(mockRepository.Object);

            var command = new CadastrarContaCorrenteCommand
            {
                Cpf = new Cpf("12345678901"),
                Nome = "Fulano de Tal",
                Senha = "123456"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.NumeroConta.Should().Be(12345);

            mockRepository.Verify(r => r.CreateAsync(It.IsAny<ContaCorrenteEntity>()), Times.Once);
        }

        [Fact]
        public void Validator_DeveFalharQuandoCamposObrigatoriosNaoPreenchidos()
        {
            // Arrange
            var mockRepository = new Mock<IContaRepository>();
            var validator = new CadastrarContaCorrenteCommandValidator(mockRepository.Object);

            var command = new CadastrarContaCorrenteCommand
            {
                Cpf = new Cpf(""),
                Nome = "",
                Senha = ""
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Cpf.Numero");
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Nome");
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Senha");
        }
    }
}