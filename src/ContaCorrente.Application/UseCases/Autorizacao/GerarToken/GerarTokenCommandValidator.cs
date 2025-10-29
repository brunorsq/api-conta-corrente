using FluentValidation;

namespace ContaCorrente.Application.UseCases.Autorizacao.GerarToken
{
    public class GerarTokenCommandValidator : AbstractValidator<GerarTokenCommand>
    {
        public GerarTokenCommandValidator()
        {
            RuleFor(x => x.Usuario)
                .NotEmpty().WithMessage("Usuário é obrigatório");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória");
        }
    }
}
