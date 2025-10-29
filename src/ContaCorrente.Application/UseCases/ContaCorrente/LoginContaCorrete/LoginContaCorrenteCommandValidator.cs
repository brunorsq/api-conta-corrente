using FluentValidation;

namespace ContaCorrente.Application.UseCases.ContaCorrente.LoginContaCorrente
{
    public class LoginContaCorrenteCommandValidator : AbstractValidator<LoginContaCorrenteCommand>
    {
        public LoginContaCorrenteCommandValidator()
        {
            RuleFor(x => x.Usuario)
                .NotEmpty().WithMessage("Usuário é obrigatório");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória");
        }
    }
}
