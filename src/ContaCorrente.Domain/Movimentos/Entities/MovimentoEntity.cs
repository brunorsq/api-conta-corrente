using ContaCorrente.Domain._Shared;
using ContaCorrente.Domain._Shared.Enums;
using ContaCorrente.Domain.Movimentos.Validators;

namespace ContaCorrente.Domain.Movimentos.Entities
{
    public class MovimentoEntity
    {
        public string? IdMovimento { get; protected set; }
        public string? IdContaCorrente { get; protected set; }
        public string? DataMovimento { get; protected set; }
        public TipoMovimentoEnum? TipoMovimento { get; protected set; }
        public decimal? Valor { get; protected set; }


        public async Task<MovimentoEntity> Create(string idContaCorrente, TipoMovimentoEnum? tipoMovimento, decimal? valor)
        {
            var validator = ValidatorProvider.Instance
                .GetValidators<MovimentoValidator>();

            var validate = await validator.CreateValidator(idContaCorrente);

            if (validate != null)
            {
                throw new DomainException(validate.Message, validate.ErrorType);
            }

            return new MovimentoEntity
            {
                IdMovimento = Guid.NewGuid().ToString(),
                IdContaCorrente = idContaCorrente,
                DataMovimento = DateTime.Now.ToString(),
                TipoMovimento = tipoMovimento,
                Valor = valor,
            };
        }


    }
}
