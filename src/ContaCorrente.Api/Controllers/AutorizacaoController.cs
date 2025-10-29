using ContaCorrente.Application.UseCases.Autorizacao.GerarToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaCorrente.Api.Controllers
{
    [ApiController]
    [Route("api/v1/autorizacao")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AutorizacaoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutorizacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> GerarToken([FromBody] GerarTokenCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (!result.IsSuccess)
                {
                    if (result.ErrorType.Equals("USER_UNAUTHORIZED"))
                        return Unauthorized(new
                        {
                            tipo = result.ErrorType,
                            menssagem = result.Message
                        });

                    return BadRequest(new
                    {
                        tipo = result.ErrorType,
                        menssagem = result.Message
                    });
                }
                    
                return Ok(result.Value);
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
