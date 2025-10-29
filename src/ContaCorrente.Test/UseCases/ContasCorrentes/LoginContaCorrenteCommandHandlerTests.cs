using ContaCorrente.Application.Interfaces.Utils;
using ContaCorrente.Application.UseCases.ContaCorrente.LoginContaCorrente;
using ContaCorrente.Domain.ContasCorrentes.Entities;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Test.Helpers;
using FluentAssertions;
using Moq;

namespace ContaCorrente.Test.UseCases.ContasCorrentes
{
    public class LoginContaCorrenteCommandHandlerTests
    {
        private readonly Mock<IContaRepository> _mockRepository;
        private readonly Mock<ITokenJWT> _mockTokenJWT;
        private readonly Mock<IPasswordValidator> _mockPasswordValidator;
        private readonly LoginContaCorrenteCommandHandler _handler;
        private readonly CriarContaHelper _criarContaHelper;

        public LoginContaCorrenteCommandHandlerTests()
        {
            _mockRepository = new Mock<IContaRepository>();
            _mockTokenJWT = new Mock<ITokenJWT>();
            _mockPasswordValidator = new Mock<IPasswordValidator>();

            _handler = new LoginContaCorrenteCommandHandler(
                _mockRepository.Object,
                _mockTokenJWT.Object,
                _mockPasswordValidator.Object
            );
            _criarContaHelper = new CriarContaHelper();
        }

        [Fact]
        public async Task Handle_LoginComSucesso_DeveRetornarToken()
        {
            // Arrange
            var comando = new LoginContaCorrenteCommand
            {
                Usuario = "12345678901",
                Senha = "senha123"
            };

            var contaFake = await _criarContaHelper.CriarContaParaTeste();

            _mockRepository.Setup(r => r.BuscarContaCorrenteAsync(It.IsAny<string>()))
                           .ReturnsAsync(contaFake);

            _mockPasswordValidator.Setup(v => v.ValidarSenha(comando.Senha, contaFake.Senha, contaFake.Salt))
                                  .Returns(true);

            _mockTokenJWT.Setup(t => t.GerarToken(It.IsAny<string>()))
                         .Returns("TOKEN_FAKE");

            // Act
            var result = await _handler.Handle(comando, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Token.Should().Be("TOKEN_FAKE");

            _mockRepository.Verify(r => r.BuscarContaCorrenteAsync(It.IsAny<string>()), Times.Once);
            _mockPasswordValidator.Verify(v => v.ValidarSenha(comando.Senha, contaFake.Senha, contaFake.Salt), Times.Once);
            _mockTokenJWT.Verify(t => t.GerarToken(contaFake.IdContaCorrente), Times.Once);
        }

        [Fact]
        public async Task Handle_ContaNaoEncontrada_DeveRetornarErro()
        {
            // Arrange
            var comando = new LoginContaCorrenteCommand
            {
                Usuario = "12345678901",
                Senha = "senha123"
            };

            _mockRepository.Setup(r => r.BuscarContaCorrenteAsync(It.IsAny<string>()))
                           .ReturnsAsync((ContaCorrenteEntity)null);

            // Act
            var result = await _handler.Handle(comando, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorType.Should().Be("USER_NOTFOUND");
            result.Message.Should().Be("Conta não encontrada.");
        }

        [Fact]
        public async Task Handle_SenhaInvalida_DeveRetornarErro()
        {
            // Arrange
            var comando = new LoginContaCorrenteCommand
            {
                Usuario = "12345678901",
                Senha = "senhaErrada"
            };

            var contaFake = await _criarContaHelper.CriarContaParaTeste(senha: "senhaCorreta");


            _mockRepository.Setup(r => r.BuscarContaCorrenteAsync(It.IsAny<string>()))
                           .ReturnsAsync(contaFake);

            _mockPasswordValidator.Setup(v => v.ValidarSenha(comando.Senha, contaFake.Senha, contaFake.Salt))
                                  .Returns(false);

            // Act
            var result = await _handler.Handle(comando, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorType.Should().Be("USER_UNAUTHORIZED");
            result.Message.Should().Be("Usuário não autorizado.");
        }
    }
}