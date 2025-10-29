using ContaCorrente.Domain._Shared;
using ContaCorrente.Domain._Shared.Enums;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain.Movimentos.Entities;
using ContaCorrente.Domain.Movimentos.Repository;
using MediatR;

namespace ContaCorrente.Application.UseCases.Movimento.AdicionarMovimento
{
    public class AdicionarMovimentoCommandHandler : IRequestHandler<AdicionarMovimentoCommand, Result>
    {
        private readonly IContaRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public AdicionarMovimentoCommandHandler(IContaRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
        }

        public async Task<Result> Handle(AdicionarMovimentoCommand request, CancellationToken cancellationToken)
        {
            if (request.Valor <= 0)
                return Result<bool>.Failure("INVALID_VALUE", "Apenas valores positivos são permitidos");

            var tipoMovimentoResult = request.TipoMovimento?.ToUpper() switch
            {
                "C" => Result<TipoMovimentoEnum>.Success(TipoMovimentoEnum.Credito),
                "D" => Result<TipoMovimentoEnum>.Success(TipoMovimentoEnum.Debito),
                _ => Result<TipoMovimentoEnum>.Failure("INVALID_TYPE", "Apenas os tipos “débito” ou “crédito” são permitidos")
            };

            if (!tipoMovimentoResult.IsSuccess)
                return Result<bool>.Failure(tipoMovimentoResult.ErrorType, tipoMovimentoResult.Message);

            TipoMovimentoEnum tipoMovimento = tipoMovimentoResult.Value;

            var contaUsuario = await _contaCorrenteRepository.BuscarContaCorrenteAsync(request.Usuario);
            var contaNumero = await _contaCorrenteRepository.BuscarContaCorrenteAsync(request.NumeroConta.ToString());

            if (contaUsuario == null)
                return Result<bool>.Failure("INVALID_ACCOUNT", "Conta não encontrada.");

            if (contaUsuario.Ativo == AtivoInativoEnum.Inativo)
                return Result<bool>.Failure("INACTIVE_ACCOUNT", "Conta está inativa.");

            if (contaNumero != null && contaNumero.Numero != contaUsuario.Numero && tipoMovimento != TipoMovimentoEnum.Credito)
                return Result<bool>.Failure("INVALID_TYPE", "Apenas o tipo “crédito” pode ser aceito caso o número da conta seja diferente do usuário logado.");

            var contaMovimento = contaNumero ?? contaUsuario;

            var movimento = await new MovimentoEntity().Create(contaMovimento.IdContaCorrente, tipoMovimento, request.Valor);

            await _movimentoRepository.CreateAsync(movimento);

            return Result<bool>.Success(true);
        }
    }
}
