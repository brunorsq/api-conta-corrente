using ContaCorrente.Application.Interfaces.Utils;
using ContaCorrente.Application.UseCases.ContaCorrente.InativarContaCorrente;
using ContaCorrente.Domain.ContasCorrentes.Entities;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Test.Helpers;
using FluentAssertions;
using Moq;

namespace ContaCorrente.Test.UseCases.ContasCorrentes
{
    public class InativarContaCorrenteCommandHandlerTests
    {
        private readonly Mock<IContaRepository> _contaRepoMock;
        private readonly Mock<IPasswordValidator> _passwordValidatorMock;
        private readonly InativarContaCorrenteCommandHandler _handler;
        private readonly CriarContaHelper _criarContaHelper;

        public InativarContaCorrenteCommandHandlerTests()
        {
            _contaRepoMock = new Mock<IContaRepository>();
            _passwordValidatorMock = new Mock<IPasswordValidator>();
            _criarContaHelper = new CriarContaHelper();

            _handler = new InativarContaCorrenteCommandHandler(
                _contaRepoMock.Object,
                _passwordValidatorMock.Object
            );
        }

        [Fact]
        public async Task Handle_ContaNaoEncontrada_DeveRetornarFailure()
        {
            // Arrange
            var command = new InativarContaCorrenteCommand { Usuario = "usuario123", Senha = "senha" };
            
            _contaRepoMock.Setup(x => x.BuscarContaCorrenteAsync("usuario123"))
                          .ReturnsAsync((ContaCorrenteEntity)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorType.Should().Be("INVALID_ACCOUNT");
        }

        [Fact]
        public async Task Handle_SenhaInvalida_DeveRetornarFailure()
        {
            // Arrange
            var command = new InativarContaCorrenteCommand { Usuario = "usuario123", Senha = "senha" };
            var conta = await _criarContaHelper.CriarContaParaTeste();

            _contaRepoMock.Setup(x => x.BuscarContaCorrenteAsync("usuario123"))
                          .ReturnsAsync(conta);
            _passwordValidatorMock.Setup(x => x.ValidarSenha(command.Senha, conta.Senha, conta.Salt))
                                  .Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorType.Should().Be("USER_UNAUTHORIZED");
        }

        [Fact]
        public async Task Handle_DadosValidos_DeveInativarContaERetornarSuccess()
        {
            // Arrange
            var command = new InativarContaCorrenteCommand { Usuario = "usuario123", Senha = "senha" };
            var conta = await _criarContaHelper.CriarContaParaTeste();

            _contaRepoMock.Setup(x => x.BuscarContaCorrenteAsync("usuario123"))
                          .ReturnsAsync(conta);
            _passwordValidatorMock.Setup(x => x.ValidarSenha(command.Senha, conta.Senha, conta.Salt))
                                  .Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _contaRepoMock.Verify(x => x.InativarContaCorrenteAsync("usuario123"), Times.Once);
        }
    }
}