using FluentValidation;

namespace ContaCorrente.Application.UseCases.ContaCorrente.InativarContaCorrente
{
    public class InativarContaCorrenteCommandValidator : AbstractValidator<InativarContaCorrenteCommand>
    {
        public InativarContaCorrenteCommandValidator()
        {
            RuleFor(x => x.Usuario)
                .NotEmpty().WithMessage("Usuário não encontrado.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória");
        }
    }
}
