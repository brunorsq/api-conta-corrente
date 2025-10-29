namespace ContaCorrente.Application.DTOs.Response.ContaCorrente
{

    public class BuscarSaldoContaCorrenteResponse
    {
        public long? NumeroConta { get; set; }
        public string? Nome { get; set; }
        public DateTime? DataConsulta { get; set; } = DateTime.Now;
        public double? ValorSaldo { get; set; }
    }
}
