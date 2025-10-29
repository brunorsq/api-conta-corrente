using ContaCorrente.Domain._Shared;
using MediatR;

namespace ContaCorrente.Application.UseCases.ContaCorrente.InativarContaCorrente
{
    public class InativarContaCorrenteCommand : IRequest<Result>
    {
        public string? Usuario { get; set; }
        public string? Senha { get; set; }
    }
}
