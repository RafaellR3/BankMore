using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferencia.Application.Transferencias;

[ApiController]
[Route("api/[controller]")]
public class TransferenciaController : ControllerBase
{
    private readonly IAplicTransferencia _aplic;

    public TransferenciaController(IAplicTransferencia aplic)
    {
        _aplic = aplic;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Efetuar([FromBody] TransferenciaDto dto)
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var ret = await _aplic.EfetuarAsync(dto.ContaOrigem, dto.ContaDestino, dto.Valor, token);
            if (!ret.Sucesso)
                return BadRequest(new
                {
                    type = ret.TipoErro,
                    message = ret.Mensagem
                });
            return NoContent(); 
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid(); 
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}

public class TransferenciaDto
{
    public string ContaOrigem { get; set; }
    public string ContaDestino { get; set; }
    public decimal Valor { get; set; }
}