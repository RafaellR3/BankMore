using Conta.Application.Contas;
using Conta.Domain.Contas;
using Conta.Domain.Contas.Dto;
using Conta.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conta.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IAplicConta _aplic;
        private readonly IRepConta _repConta;

        public ContaCorrenteController(IAplicConta aplic, 
                                       IRepConta repConta)
        {
            _aplic = aplic;
            _repConta = repConta;
        }


        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ContaDto conta)
        {
            if (!ValidadorCfp.Validar(conta.Cpf))
            {
                return BadRequest(new
                {
                    type = "INVALID_DOCUMENT",
                    message = "O CPF informado é inválido."
                });
            }

            var ret = await _aplic.Criar(conta);
            return Ok(ret);
        }

        [Authorize]
        [HttpPut("Inativar")]
        public async Task<IActionResult> Inativar([FromBody] InativarDto dto)
        {
            var conta = await _repConta.ObterPorNumeroAsync(dto.Numero);
            if (conta == null)
            {
                return NotFound(new
                {
                    type = "INVALID_ACCOUNT",
                    message = "Conta não encontrada."
                });
            }

            if (!conta.VerificarSenha(dto.Senha))
            {
                return BadRequest(new
                {
                    type = "INVALID_PASSWORD",
                    message = "Senha inválida."
                });
            }

            _aplic.Inativar(conta);

            return NoContent();
        }

        [Authorize]
        [HttpGet("{numeroConta}/saldo")]
        public async Task<IActionResult> ConsultarSaldo([FromRoute] int numeroConta)

        {
            var resultado = await _aplic.ConsultarSaldoAsync(numeroConta);

            if (!resultado.Sucesso)
            {
                if (resultado.TipoErro == "INVALID_ACCOUNT" || resultado.TipoErro == "INACTIVE_ACCOUNT")
                    return BadRequest(new { erro = resultado.TipoErro, mensagem = resultado.Mensagem });

                if (resultado.TipoErro == "TOKEN_INVALID")
                    return Forbid(); 
            }

            return Ok(resultado.Dados);
        }

    }
}

