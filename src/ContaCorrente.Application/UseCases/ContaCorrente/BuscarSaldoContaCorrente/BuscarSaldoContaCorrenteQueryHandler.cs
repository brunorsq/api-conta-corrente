using ContaCorrente.Application.Interfaces.Utils;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain._Shared;
using MediatR;
using ContaCorrente.Application.DTOs.Response.ContaCorrente;
using ContaCorrente.Application.UseCases.ContaCorrente.BuscalSaldoContaCorrente;
using ContaCorrente.Domain._Shared.Enums;

namespace ContaCorrente.Application.UseCases.ContaCorrente.BuscalSaldoContaCorrente
{
    public class BuscarSaldoContaCorrenteQueryHandler : IRequestHandler<BuscarSaldoContaCorrenteQuery, Result<BuscarSaldoContaCorrenteResponse>>
    {
        private readonly IContaRepository _contaCorrenteRepository;

        public BuscarSaldoContaCorrenteQueryHandler(IContaRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<Result<BuscarSaldoContaCorrenteResponse>> Handle(BuscarSaldoContaCorrenteQuery request, CancellationToken cancellationToken)
        {
            var conta = await _contaCorrenteRepository.BuscarContaCorrenteAsync(request.Usuario);

            if (conta == null)
                return Result<BuscarSaldoContaCorrenteResponse>.Failure("INVALID_ACCOUNT", "Conta não encontrada.");

            if (conta.Ativo == AtivoInativoEnum.Inativo)
                return Result<BuscarSaldoContaCorrenteResponse>.Failure("INACTIVE_ACCOUNT", "Conta está inativa.");

            var saldo = await _contaCorrenteRepository.BuscarSaldoContaCorrenteAsync(conta.IdContaCorrente);

            var retorno = new BuscarSaldoContaCorrenteResponse()
            {
                NumeroConta = conta.Numero,
                Nome = conta.Nome,
                ValorSaldo = saldo ?? 0
            };
            return Result<BuscarSaldoContaCorrenteResponse>.Success(retorno);
        }
    }
}
