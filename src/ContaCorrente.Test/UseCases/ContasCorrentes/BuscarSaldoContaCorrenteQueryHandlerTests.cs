using ContaCorrente.Application.UseCases.ContaCorrente.BuscalSaldoContaCorrente;
using ContaCorrente.Domain._Shared.Enums;
using ContaCorrente.Domain.ContasCorrentes.Entities;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Test.Helpers;
using FluentAssertions;
using Moq;

namespace ContaCorrente.Test.UseCases.ContasCorrentes
{
    public class BuscarSaldoContaCorrenteHandlerTests
    {
        private readonly Mock<IContaRepository> _contaRepositoryMock;
        private readonly BuscarSaldoContaCorrenteQueryHandler _handler;
        private readonly CriarContaHelper _criarContaHelper;

        public BuscarSaldoContaCorrenteHandlerTests()
        {
            _contaRepositoryMock = new Mock<IContaRepository>();
            _handler = new BuscarSaldoContaCorrenteQueryHandler(_contaRepositoryMock.Object);
            _criarContaHelper = new CriarContaHelper();
        }

        [Fact]
        public async Task Handle_ContaNaoEncontrada_RetornaInvalidAccount()
        {
            // Arrange
            var query = new BuscarSaldoContaCorrenteQuery { Usuario = "12345678900" };
            _contaRepositoryMock
                .Setup(r => r.BuscarContaCorrenteAsync(query.Usuario))
                .ReturnsAsync((ContaCorrenteEntity)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorType.Should().Be("INVALID_ACCOUNT");
            result.Message.Should().Be("Conta não encontrada.");
        }

        [Fact]
        public async Task Handle_ContaInativa_RetornaInactiveAccount()
        {
            // Arrange
            var query = new BuscarSaldoContaCorrenteQuery { Usuario = "12345678900" };

            var conta = await _criarContaHelper.CriarContaParaTeste();

            var contaInativa = _criarContaHelper.InativarContaTeste(conta);

            _contaRepositoryMock
                .Setup(r => r.BuscarContaCorrenteAsync(query.Usuario))
                .ReturnsAsync(contaInativa);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorType.Should().Be("INACTIVE_ACCOUNT");
            result.Message.Should().Be("Conta está inativa.");
        }

        [Fact]
        public async Task Handle_ContaAtiva_ComSaldo_RetornaSucesso()
        {
            // Arrange
            var query = new BuscarSaldoContaCorrenteQuery { Usuario = "12345678900" };
            var conta = await _criarContaHelper.CriarContaParaTeste();

            _contaRepositoryMock
                .Setup(r => r.BuscarContaCorrenteAsync(query.Usuario))
                .ReturnsAsync(conta);

            _contaRepositoryMock
                .Setup(r => r.BuscarSaldoContaCorrenteAsync(conta.IdContaCorrente))
                .ReturnsAsync(500.50);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.NumeroConta.Should().Be(conta.Numero);
            result.Value.Nome.Should().Be(conta.Nome);
            result.Value.ValorSaldo.Should().Be(500.50);
        }

        [Fact]
        public async Task Handle_ContaAtiva_SemMovimentos_RetornaSaldoZero()
        {
            // Arrange
            var query = new BuscarSaldoContaCorrenteQuery { Usuario = "12345678900" };
            var conta = await _criarContaHelper.CriarContaParaTeste();

            _contaRepositoryMock
                .Setup(r => r.BuscarContaCorrenteAsync(query.Usuario))
                .ReturnsAsync(conta);

            // Simula que não há movimentos → retorna null
            _contaRepositoryMock
                .Setup(r => r.BuscarSaldoContaCorrenteAsync(conta.IdContaCorrente))
                .ReturnsAsync((double?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.NumeroConta.Should().Be(conta.Numero);
            result.Value.Nome.Should().Be(conta.Nome);
            result.Value.ValorSaldo.Should().Be(0); // deve usar o ?? 0 do handler
        }
    }
}