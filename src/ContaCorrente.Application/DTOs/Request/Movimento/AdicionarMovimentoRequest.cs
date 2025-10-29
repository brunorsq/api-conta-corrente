using ContaCorrente.Domain._Shared.Enums;

namespace ContaCorrente.Application.DTOs.Request.Movimento
{
    public class AdicionarMovimentoRequest
    {
        public long? NumeroConta { get; set; }
        public decimal? Valor { get; set; }
        public string? TipoMovimento { get; set; }
    }
}
