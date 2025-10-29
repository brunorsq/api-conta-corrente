using ContaCorrente.Domain._Shared;
using ContaCorrente.Domain._Shared.Enums;
using ContaCorrente.Domain.ContasCorrentes.Repository;

namespace ContaCorrente.Domain.Movimentos.Validators
{
    public class MovimentoValidator : DomainValidator
    {
        private readonly IContaRepository _contaCorrenteRepository;

        public MovimentoValidator(IContaRepository movimentoRepository)
        {
            _contaCorrenteRepository = movimentoRepository;
        }

        public async Task<DomainValidator?> CreateValidator(string idContaCorrente)
        {
            var conta = await _contaCorrenteRepository.BuscarContaCorrenteAsync(idContaCorrente);

            if (conta == null)
                return new DomainValidator()
                {
                    ErrorType = "INVALID_ACCOUNT",
                    Message = "Conta não encontrada."
                };

            if (conta.Ativo != AtivoInativoEnum.Ativo)
                return new DomainValidator()
                {
                    ErrorType = "INACTIVE_ACCOUNT",
                    Message = "Conta está inativa."

                };

            return null;
        }
    }
}
