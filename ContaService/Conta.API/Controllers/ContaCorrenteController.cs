using Conta.Application.Contas.Queries;
using Conta.Domain.Contas.Dto;
using Conta.Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Conta.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ContaDto dto)
        {
            var numeroConta = await _mediator.Send(new CriarContaCommand(dto));
            return Ok(new { NumeroConta = numeroConta });
        }

        [Authorize]
        [HttpPost("inativar")]
        public async Task<IActionResult> Inativar( [FromBody] InativarDto dto)
        {

            var resultado = await _mediator.Send(new InativarContaCommand(dto.Numero, dto.Senha));

            if (!resultado.Sucesso)
                return BadRequest(new { type = resultado.TipoErro, message = resultado.Mensagem });

            return NoContent();
        }


        [Authorize]
        [HttpGet("{idConta}/saldo")]
        public async Task<IActionResult> ConsultarSaldo(Guid idConta)
        {
            var resultado = await _mediator.Send(new ConsultarSaldoQuery(idConta));

            if (!resultado.Sucesso)
                return BadRequest(new { type = resultado.TipoErro, message = resultado.Mensagem });

            return Ok(resultado.Dados);
        }

        [Authorize]
        [HttpGet("{idConta}")]
        public async Task<IActionResult> ObterConta(Guid idConta)
        {
            var resultado = await _mediator.Send(new ObterContaQuery(idConta));

            if (!resultado.Sucesso)
                return NotFound(new { type = resultado.TipoErro, message = resultado.Mensagem });

            return Ok(resultado.Dados);
        }

    }
}

