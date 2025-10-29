using ContaCorrente.Application.DTOs.Response.Autorizacao;
using ContaCorrente.Application.Interfaces.Utils;
using ContaCorrente.Domain._Shared;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ContaCorrente.Application.UseCases.Autorizacao.GerarToken
{
    public class GerarTokenCommandHandler : IRequestHandler<GerarTokenCommand, Result<GerarTokenResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenJWT _tokenJWT;
        public GerarTokenCommandHandler(IConfiguration configuration, ITokenJWT tokenJWT)
        {
            _configuration = configuration;
            _tokenJWT = tokenJWT;
        }

        public async Task<Result<GerarTokenResponse>> Handle(GerarTokenCommand request, CancellationToken cancellationToken)
        {
            if (request.Usuario != _configuration["Authorization:Admin"] || request.Senha != _configuration["Authorization:Senha"])
                return Result<GerarTokenResponse>.Failure("USER_UNAUTHORIZED", "Usuário não autorizado.");

            var response = new GerarTokenResponse
            {
                Token = _tokenJWT.GerarToken(request.Usuario)
            };

            return Result<GerarTokenResponse>.Success(response);
        }
    }
}
