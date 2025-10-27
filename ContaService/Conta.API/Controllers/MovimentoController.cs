using Conta.Application.Movimentos.Commands;
using Conta.Application.Movimentos.Commands.Handlers;
using Conta.Domain.Idempotencias;
using Conta.Domain.Movimentos.Dto;
using Conta.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System.Security.Claims;

namespace Conta.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] MovimentoDto dto, [FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
        {            
            var dadosAmbiente = ObterDadosAmbiente();

            if (!dto.IdConta.HasValue)
                dto.IdConta = dadosAmbiente.IdContaUsuarioLogado;

            var resultado = await _mediator.Send(new CriarMovimentoCommand(dto, idempotencyKey, dadosAmbiente));

            if (!resultado.Sucesso)
                return BadRequest(new { type = resultado.TipoErro, message = resultado.Mensagem });

            return Ok(resultado.Dados);
        }

        [HttpPost("{idMovto}/Estorno")]
        public async Task<IActionResult> Extornar([FromRoute] Guid movto)
        {
            var resultado = await _mediator.Send(new EstornarMovtoCommand(movto));
            if (!resultado.Sucesso)
                return BadRequest(new { type = resultado.TipoErro, message = resultado.Mensagem });

            return Ok(resultado.Dados);
        }


        private DadosAmbiente ObterDadosAmbiente()
        {
            var dados = new DadosAmbiente();

            dados.CpfUsuarioLogado = User.Claims.FirstOrDefault(c => c.Type == "cpf")?.Value;

            Guid idConta;
            var contaToken = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(contaToken, out idConta);
            dados.IdContaUsuarioLogado = idConta;

            return dados;
        }
    }
}
