using Conta.Application.Movimentos.Commands;
using Conta.Domain.Idempotencias;
using Conta.Domain.Movimentos.Dto;
using Conta.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            if (!dto.NumeroConta.HasValue)
                dto.NumeroConta = dadosAmbiente.NumeroContaUsuarioLogado;

            var resultado = await _mediator.Send(new CriarMovimentoCommand(dto, idempotencyKey, dadosAmbiente));

            if (!resultado.Sucesso)
                return BadRequest(new { type = resultado.TipoErro, message = resultado.Mensagem });

            return Ok(resultado.Dados);
        }

    
        private DadosAmbiente ObterDadosAmbiente()
        {
            var dados = new DadosAmbiente();

            dados.CpfUsuarioLogado = User.Claims.FirstOrDefault(c => c.Type == "cpf")?.Value;

            int numeroConta;
            var numeroContaToken = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            int.TryParse(numeroContaToken, out numeroConta);
            dados.NumeroContaUsuarioLogado = numeroConta;

            return dados;
        }
    }
}
