using ContaCorrente.Application.DTOs.Request.ContaCorrente;
using ContaCorrente.Application.DTOs.Response.ContaCorrente;
using ContaCorrente.Application.Interfaces.Utils;
using ContaCorrente.Application.UseCases.ContaCorrente.BuscalSaldoContaCorrente;
using ContaCorrente.Application.UseCases.ContaCorrente.CadastrarContaCorrente;
using ContaCorrente.Application.UseCases.ContaCorrente.InativarContaCorrente;
using ContaCorrente.Application.UseCases.ContaCorrente.LoginContaCorrente;
using ContaCorrente.Domain._Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaCorrente.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/conta-corrente")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITokenJWT _tokenJWT;

        public ContaCorrenteController(IMediator mediator, ITokenJWT tokenJWT)
        {
            _mediator = mediator;
            _tokenJWT = tokenJWT;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarAsync([FromBody] CadastrarContaCorrenteRequest request)
        {
            try
            {
                var command = new CadastrarContaCorrenteCommand()
                {
                    Cpf = new Cpf(request.Cpf),
                    Nome = request.Nome,
                    Senha = request.Senha
                };

                var result = await _mediator.Send(command);

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
            catch (DomainException ex)
            {
                var result = Result<CadastrarContaCorrenteResponse>.Failure(ex.Code, ex.Message);
                return BadRequest(new
                {
                    tipo = result.ErrorType,
                    menssagem = result.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginContaCorrenteCommand command)
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

                    if (result.ErrorType.Equals("USER_NOTFOUND"))
                        return NotFound(new
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
            catch (DomainException ex)
            {
                var result = Result<CadastrarContaCorrenteResponse>.Failure(ex.Code, ex.Message);
                return BadRequest(new
                {
                    tipo = result.ErrorType,
                    menssagem = result.Message
                });
            }
        }
        [HttpDelete("inativar")]
        public async Task<IActionResult> InativarAsync([FromBody] InativarContaCorrenteRequest request, [FromHeader(Name = "Login-Token")] string token)
        {
            try
            {
                var usuario = _tokenJWT.BuscarUsuarioToken(token);

                if (string.IsNullOrEmpty(usuario))
                    return Forbid();

                var command = new InativarContaCorrenteCommand()
                {
                    Usuario = usuario,
                    Senha = request.Senha
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

        [HttpGet("saldo")]
        public async Task<IActionResult> BuscarSaldoAsync([FromHeader(Name = "Login-Token")] string token)
        {
            try
            {
                var usuario = _tokenJWT.BuscarUsuarioToken(token);

                if (string.IsNullOrEmpty(usuario))
                    return Forbid();

                var command = new BuscarSaldoContaCorrenteQuery()
                {
                    Usuario = usuario,
                };

                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                {
                    return BadRequest(new { tipo = result.ErrorType, mensagem = result.Message });
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
