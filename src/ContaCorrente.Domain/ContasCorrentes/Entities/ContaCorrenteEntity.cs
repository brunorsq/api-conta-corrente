using ContaCorrente.Domain.ContasCorrentes.Validators;
using ContaCorrente.Domain._Shared;
using ContaCorrente.Domain._Shared.Enums;

namespace ContaCorrente.Domain.ContasCorrentes.Entities
{
    public class ContaCorrenteEntity
    {
        public string? IdContaCorrente { get; protected set; }
        public long? Numero { get; protected set; }
        public string? Nome { get; protected set; }
        public AtivoInativoEnum? Ativo { get; protected set; }
        public string? Senha { get; protected set; }
        public string? Salt { get; protected set; }

        public async Task<ContaCorrenteEntity> Create(string? cpf, long? numero, string? nome, string? senha)
        {
            var cpfDesformatado = new Cpf(cpf);

            var validator = ValidatorProvider.Instance
                .GetValidators<ContaValidator>();

            var validate = validator.CreateValidator(cpfDesformatado.Numero);

            if (validate != null)
            {
                throw new DomainException(validate.Message, validate.ErrorType);
            }   

            var salt = Guid.NewGuid().ToString("N")[..8];
            var senhaHash = new GerarHash(senha, salt).Hash;

            return new ContaCorrenteEntity
            {
                IdContaCorrente = cpfDesformatado.Numero,
                Numero = numero,
                Nome = nome,
                Ativo = AtivoInativoEnum.Ativo,
                Senha = senhaHash,
                Salt = salt
            };
        }

        public ContaCorrenteEntity Inative(ContaCorrenteEntity conta)
        {
            conta.Ativo = AtivoInativoEnum.Inativo;

            return conta;
        }
    }
}
