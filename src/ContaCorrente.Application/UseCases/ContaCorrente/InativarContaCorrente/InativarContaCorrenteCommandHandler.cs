using ContaCorrente.Application.Interfaces.Utils;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain._Shared;
using MediatR;

namespace ContaCorrente.Application.UseCases.ContaCorrente.InativarContaCorrente
{
    public class InativarContaCorrenteCommandHandler : IRequestHandler<InativarContaCorrenteCommand, Result>
    {
        private readonly IContaRepository _contaCorrenteRepository;
        private readonly IPasswordValidator _passwordValidator;
        

        public InativarContaCorrenteCommandHandler(IContaRepository contaCorrenteRepository, IPasswordValidator passwordValidator)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _passwordValidator = passwordValidator;
        }

        public async Task<Result> Handle(InativarContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            var conta = await _contaCorrenteRepository.BuscarContaCorrenteAsync(request.Usuario);

            if (conta == null)
                return Result<bool>.Failure("INVALID_ACCOUNT", "Conta não encontrada.");

            if (!_passwordValidator.ValidarSenha(request.Senha, conta.Senha, conta.Salt))
                return Result<bool>.Failure("USER_UNAUTHORIZED", "Usuário não autorizado.");

            await _contaCorrenteRepository.InativarContaCorrenteAsync(request.Usuario);

            return Result<bool>.Success(true);
        }
    }
}
