using ContaCorrente.Domain.ContasCorrentes.Repository;
using FluentValidation;

namespace ContaCorrente.Application.UseCases.ContaCorrente.CadastrarContaCorrente
{
    public class CadastrarContaCorrenteCommandValidator : AbstractValidator<CadastrarContaCorrenteCommand>
    {
        private readonly IContaRepository _repository;

        public CadastrarContaCorrenteCommandValidator(IContaRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Cpf.Numero)
                .NotEmpty().WithMessage("CPF é obrigatório");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória");
        }        
    }
}
