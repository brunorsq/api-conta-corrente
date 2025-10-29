using ContaCorrente.Application.DTOs.Response.ContaCorrente;
using ContaCorrente.Domain.ContasCorrentes.Entities;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain._Shared;
using MediatR;

namespace ContaCorrente.Application.UseCases.ContaCorrente.CadastrarContaCorrente
{
    public class CadastrarContaCorrenteCommandHandler : IRequestHandler<CadastrarContaCorrenteCommand, Result<CadastrarContaCorrenteResponse>>
    {
        private readonly IContaRepository _contaCorrenteRepository;

        public CadastrarContaCorrenteCommandHandler(IContaRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<Result<CadastrarContaCorrenteResponse>> Handle(CadastrarContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            long numeroConta = await _contaCorrenteRepository.ObterProximoNumeroAsync();

            var conta = await new ContaCorrenteEntity().Create(request.Cpf.Numero, numeroConta, request.Nome, request.Senha);

            await _contaCorrenteRepository.CreateAsync(conta);

            var response = new CadastrarContaCorrenteResponse
            {
                NumeroConta = conta.Numero,
            };

            return Result<CadastrarContaCorrenteResponse>.Success(response);
        }
    }
}
