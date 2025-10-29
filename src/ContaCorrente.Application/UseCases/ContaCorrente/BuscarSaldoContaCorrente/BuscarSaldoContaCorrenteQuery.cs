using ContaCorrente.Application.DTOs.Response.ContaCorrente;
using ContaCorrente.Domain._Shared;
using MediatR;

namespace ContaCorrente.Application.UseCases.ContaCorrente.BuscalSaldoContaCorrente
{
    public class BuscarSaldoContaCorrenteQuery : IRequest<Result<BuscarSaldoContaCorrenteResponse>>
    {
        public string? Usuario { get; set; }
    }
}
