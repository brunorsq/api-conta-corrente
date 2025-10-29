using ContaCorrente.Application.DTOs.Response.Autorizacao;
using ContaCorrente.Application.Interfaces.Utils;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Domain._Shared;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ContaCorrente.Application.UseCases.ContaCorrente.LoginContaCorrente
{
    public class LoginContaCorrenteCommandHandler : IRequestHandler<LoginContaCorrenteCommand, Result<GerarTokenResponse>>
    {
        private readonly IContaRepository _contaCorrenteRepository;
        private readonly ITokenJWT _tokenJWT;
        private readonly IPasswordValidator _passwordValidator;

        public LoginContaCorrenteCommandHandler(IContaRepository contaCorrenteRepository, ITokenJWT tokenJWT, IPasswordValidator passwordValidator)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _tokenJWT = tokenJWT;
            _passwordValidator = passwordValidator;
        }

        public async Task<Result<GerarTokenResponse>> Handle(LoginContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            string cpf = null;
            if (request.Usuario.Length >= 11)
            {
                cpf = new Cpf(request.Usuario).Numero;
            }

            var conta = await _contaCorrenteRepository.BuscarContaCorrenteAsync(cpf ?? request.Usuario);

            if (conta == null)
                return Result<GerarTokenResponse>.Failure("USER_NOTFOUND", "Conta não encontrada.");

            if (!_passwordValidator.ValidarSenha(request.Senha, conta.Senha, conta.Salt))
                return Result<GerarTokenResponse>.Failure("USER_UNAUTHORIZED", "Usuário não autorizado.");

            var response = new GerarTokenResponse
            {
                Token = _tokenJWT.GerarToken(conta.IdContaCorrente.ToString()),
            };

            return Result<GerarTokenResponse>.Success(response);
        }
    }
}
