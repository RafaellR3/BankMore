using Conta.Application.Movimentos;
using Conta.Domain.Idempotencias;
using Conta.Domain.Movimentos;
using Conta.Domain.Movimentos.Dto;
using Conta.Infrastructure;
using Conta.Infrastructure.Resultados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Conta.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentoController : ControllerBase
    {
        private readonly IRepIdempotencia _repIdempotencia;
        private readonly IAplicMovimento _aplic;
        private readonly IRepMovimento _repMovimento;
        public MovimentoController(IRepIdempotencia repIdempotencia,
                                   IAplicMovimento aplic,
                                   IRepMovimento repMovimento)
        {
            _repIdempotencia = repIdempotencia;
            _aplic = aplic;
            _repMovimento = repMovimento;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Criar(MovimentoDto dto, [FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
        {
            var existente = await _repIdempotencia.ObterAsync(idempotencyKey);
            if (existente != null)
                return Content(existente.Resultado, "application/json");
            var dadosAmbiente = ObterDadosAmbiente();

            if (!dto.NumeroConta.HasValue)
                dto.NumeroConta = dadosAmbiente.NumeroContaUsuarioLogado;

            var resultado = await _aplic.Criar(dto, idempotencyKey, dadosAmbiente);
            if(!resultado.Sucesso)
                return BadRequest(new { erro = resultado.TipoErro, mensagem = resultado.Mensagem });

            return NoContent(); 
        }

        [Authorize]
        [HttpGet("{idContaCorrente}")]
        public async Task<IActionResult> Listar(string idContaCorrente)
        {
            var movimentos = await _repMovimento.ListarPorContaAsync(idContaCorrente);
            return Ok(movimentos);
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
