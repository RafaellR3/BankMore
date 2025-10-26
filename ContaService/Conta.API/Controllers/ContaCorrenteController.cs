using Conta.Application;
using Conta.Domain.Contas;
using Conta.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Conta.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IAplicConta _aplic;

        public ContaCorrenteController(IAplicConta aplic, IRepConta repository)
        {
            _aplic = aplic;
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
    }
}

