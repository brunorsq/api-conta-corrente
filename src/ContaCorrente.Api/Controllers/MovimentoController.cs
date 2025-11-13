using ContaCorrente.Application.DTOs.Request.Movimento;
using ContaCorrente.Application.Interfaces.Utils;
using ContaCorrente.Application.UseCases.Movimento.AdicionarMovimento;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaCorrente.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/movimento")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class MovimentoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITokenJWT _tokenJWT;

        public MovimentoController(IMediator mediator, ITokenJWT tokenJWT)
        {
            _mediator = mediator;
            _tokenJWT = tokenJWT;
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> AdicionarAsync([FromBody] AdicionarMovimentoRequest request, [FromHeader(Name = "Login-Token")] string token)
        {
            try
            {
                var usuario = _tokenJWT.BuscarUsuarioToken(token);

                if (string.IsNullOrEmpty(usuario))
                    return Forbid();

                var command = new AdicionarMovimentoCommand()
                {
                    Usuario = usuario,
                    NumeroConta = request.NumeroConta,
                    TipoMovimento = request.TipoMovimento,
                    Valor = request.Valor,
                };

                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                {
                    return BadRequest(new { tipo = result.ErrorType, mensagem = result.Message });
                }

                return NoContent();
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new
                {
                    Message = "Validação falhou",
                    Errors = ex.Errors.Select(e => e.ErrorMessage)
                });
            }
        }
    }
}
