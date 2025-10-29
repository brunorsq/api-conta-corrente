using FluentValidation;

namespace ContaCorrente.Application.UseCases.Movimento.AdicionarMovimento
{
    public class AdicionarMovimimentoCommandValidator : AbstractValidator<AdicionarMovimentoCommand>
    {
        public AdicionarMovimimentoCommandValidator()
        {
            RuleFor(x => x.Valor)
                .NotEmpty().WithMessage("Valor é obrigatório");

            RuleFor(x => x.TipoMovimento)
                .NotNull().WithMessage("Tipo de movimento é obrigatório");
        }
    }
}
