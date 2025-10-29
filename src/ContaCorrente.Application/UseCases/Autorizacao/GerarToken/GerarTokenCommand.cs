using ContaCorrente.Application.DTOs.Response.Autorizacao;
using ContaCorrente.Domain._Shared;
using MediatR;

namespace ContaCorrente.Application.UseCases.Autorizacao.GerarToken
{
    public class GerarTokenCommand : IRequest<Result<GerarTokenResponse>>
    {
        public string? Usuario { get; set; }
        public string? Senha { get; set; }
    }
}
