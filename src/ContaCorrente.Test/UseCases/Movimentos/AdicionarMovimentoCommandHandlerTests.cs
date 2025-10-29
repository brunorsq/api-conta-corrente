using ContaCorrente.Application.UseCases.Movimento.AdicionarMovimento;
using ContaCorrente.Domain._Shared;
using ContaCorrente.Domain.ContasCorrentes.Entities;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain.ContasCorrentes.Validators;
using ContaCorrente.Domain.Movimentos.Entities;
using ContaCorrente.Domain.Movimentos.Repository;
using ContaCorrente.Domain.Movimentos.Validators;
using ContaCorrente.Infra.Data.Repository.ContaCorrente;
using ContaCorrente.Test.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ContaCorrente.Test.UseCases.Movimentos
{
    public class AdicionarMovimentoHandlerTests
    {
        private readonly Mock<IContaRepository> _contaRepositoryMock;
        private readonly Mock<IMovimentoRepository> _movimentoRepositoryMock;
        private readonly AdicionarMovimentoCommandHandler _handler;
        private readonly AdicionarMovimimentoCommandValidator _validator;
        private readonly CriarContaHelper _criarContaHelper;

        public AdicionarMovimentoHandlerTests()
        {
            _contaRepositoryMock = new Mock<IContaRepository>();
            _movimentoRepositoryMock = new Mock<IMovimentoRepository>();
            _criarContaHelper = new CriarContaHelper();
            
            _handler = new AdicionarMovimentoCommandHandler(
                _contaRepositoryMock.Object,
                _movimentoRepositoryMock.Object
            );

            _validator = new AdicionarMovimimentoCommandValidator();
            ValidatorProviderHelper.InitializeWithMocks(_contaRepositoryMock);
        }

        [Fact]
        public void Validator_ComValorVazio_RetornaErro()
        {
            // Arrange
            var command = new AdicionarMovimentoCommand
            {
                Valor = null,
                TipoMovimento = "C"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Valor é obrigatório");
        }

        [Fact]
        public void Validator_ComTipoMovimentoNulo_RetornaErro()
        {
            // Arrange
            var command = new AdicionarMovimentoCommand
            {
                Valor = 100,
                TipoMovimento = null
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Tipo de movimento é obrigatório");
        }

        [Fact]
        public void Validator_ComValoresValidos_RetornaValido()
        {
            // Arrange
            var command = new AdicionarMovimentoCommand
            {
                Valor = 100,
                TipoMovimento = "C"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ValorNegativo_RetornaInvalidValue()
        {
            // Arrange
            var request = new AdicionarMovimentoCommand
            {
                Valor = -10,
                TipoMovimento = "C",
                Usuario = "12345678900",
                NumeroConta = 123
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().Be(false);
            result.ErrorType.Should().Be("INVALID_VALUE");
        }

        [Theory]
        [InlineData("X")]
        [InlineData(null)]
        public async Task Handle_TipoMovimentoInvalido_RetornaInvalidType(string tipo)
        {
            // Arrange
            var request = new AdicionarMovimentoCommand
            {
                Valor = 100,
                TipoMovimento = tipo,
                Usuario = "user1",
                NumeroConta = 123
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().Be(false);
            result.ErrorType.Should().Be("INVALID_TYPE");
        }

        [Fact]
        public async Task Handle_ContaUsuarioNaoEncontrada_RetornaInvalidAccount()
        {
            // Arrange
            var request = new AdicionarMovimentoCommand
            {
                Valor = 100,
                TipoMovimento = "C",
                Usuario = "12345678900",
                NumeroConta = 123
            };

            _contaRepositoryMock.Setup(r => r.BuscarContaCorrenteAsync("12345678900"))
                .ReturnsAsync((ContaCorrenteEntity)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().Be(false);
            result.ErrorType.Should().Be("INVALID_ACCOUNT");
        }

        [Fact]
        public async Task Handle_ContaInativa_RetornaInactiveAccount()
        {
            // Arrange
            var request = new AdicionarMovimentoCommand
            {
                Valor = 100,
                TipoMovimento = "C",
                Usuario = "12345678900",
                NumeroConta = 123
            };

            var conta = await _criarContaHelper.CriarContaParaTeste();
            var contaInativa = _criarContaHelper.InativarContaTeste(conta);

            _contaRepositoryMock.Setup(r => r.BuscarContaCorrenteAsync("12345678900"))
                .ReturnsAsync(contaInativa);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().Be(false);
            result.ErrorType.Should().Be("INACTIVE_ACCOUNT");
        }

        [Fact]
        public async Task Handle_TipoDebitoContaDiferente_RetornaInvalidType()
        {
            // Arrange
            var request = new AdicionarMovimentoCommand
            {
                Valor = 100,
                TipoMovimento = "D",
                Usuario = "12345678900",
                NumeroConta = 100002
            };

            var conta1 = await _criarContaHelper.CriarContaParaTeste();
            var conta2 = await _criarContaHelper.CriarContaParaTeste(id: "00987654321", numero: 100002);

            _contaRepositoryMock.Setup(r => r.BuscarContaCorrenteAsync("12345678900"))
                .ReturnsAsync(conta1);

            _contaRepositoryMock.Setup(r => r.BuscarContaCorrenteAsync("100002"))
                .ReturnsAsync(conta2);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().Be(false);
            result.ErrorType.Should().Be("INVALID_TYPE");
        }

        [Fact]
        public async Task Handle_TudoValido_RetornaSucesso()
        {
            // Arrange
            var request = new AdicionarMovimentoCommand
            {
                Valor = 100,
                TipoMovimento = "C",
                Usuario = "12345678900",
                NumeroConta = 100001
            };

            var contaUsuario = await _criarContaHelper.CriarContaParaTeste();

            _contaRepositoryMock.Setup(r => r.BuscarContaCorrenteAsync("12345678900"))
                .ReturnsAsync(contaUsuario);

            _contaRepositoryMock.Setup(r => r.BuscarContaCorrenteAsync("100001"))
                .ReturnsAsync(contaUsuario);

            _movimentoRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<MovimentoEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().Be(true);
        }
    }
}