using ContaCorrente.Domain._Shared;
using ContaCorrente.Domain._Shared.Enums;
using MediatR;

namespace ContaCorrente.Application.UseCases.Movimento.AdicionarMovimento
{
    public class AdicionarMovimentoCommand : IRequest<Result>
    {
        public string? Usuario { get; set; }
        public long? NumeroConta { get; set; }
        public decimal? Valor { get; set; }
        public string? TipoMovimento { get; set; }
    }
}
