using ContaCorrente.Application.DTOs.Response.ContaCorrente;
using ContaCorrente.Domain._Shared;
using MediatR;

namespace ContaCorrente.Application.UseCases.ContaCorrente.CadastrarContaCorrente
{
    public class CadastrarContaCorrenteCommand : IRequest<Result<CadastrarContaCorrenteResponse>>
    {
        public Cpf? Cpf { get; set; }
        public string? Nome { get; set; }
        public string? Senha { get; set; }
    }
}
