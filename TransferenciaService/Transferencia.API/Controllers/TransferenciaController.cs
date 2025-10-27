using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferencia.Application.Transferencias.Commands;

[ApiController]
[Route("api/[controller]")]
public class TransferenciaController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransferenciaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Criar(
    [FromBody] TransferenciaDto dto,
    [FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
    {
        var command = new CriarTransferenciaCommand(
            dto.ContaOrigem,
            dto.ContaDestino,
            dto.Valor,
            idempotencyKey
        );

        var resultado = await _mediator.Send(command);

        if (!resultado.Sucesso)
            return BadRequest(new { type = resultado.TipoErro, message = resultado.Mensagem });

        return Ok(resultado.Dados);
    }

}

public class TransferenciaDto
{
    public int ContaOrigem { get; set; }
    public int ContaDestino { get; set; }
    public decimal Valor { get; set; }
}