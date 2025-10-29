using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain._Shared;

namespace ContaCorrente.Domain.ContasCorrentes.Validators
{
    public class ContaValidator : DomainValidator
    {
        private readonly IContaRepository _contaCorrenteRepository;

        public ContaValidator(IContaRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public DomainValidator? CreateValidator(string idContaCorrente)
        {
            if (_contaCorrenteRepository.ExisteContaPorCpf(idContaCorrente))
                return new DomainValidator()
                {
                    ErrorType = "INVALID_DOCUMENT",
                    Message = "CPF já possui uma conta."
                
                };

            return null;
        }

    }
}
