using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuario.Application;
using Usuario.Application.Dto;
using Usuario.Application.View;
using Usuario.Domain.Usuarios;

namespace Usuario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IAplicUser _aplic;
    private readonly IRepUsuario _repository;

    public UsuarioController(IAplicUser aplic, IRepUsuario repository)
    {
        _aplic = aplic;
        _repository = repository;
    }


    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] UserDto usuario)
    {
        var ret =  await _aplic.CriarUsuario(usuario);
        return Ok(ret);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Obter(Guid id)
    {
        var usuario = await _repository.ObterPorIdAsync(id);
        if (usuario == null) return NotFound();
        return Ok(UserView.Novo(usuario));
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var usuarios = await _repository.ListarAsync();
        return Ok(UserView.Novo(usuarios.ToList()));
    }










}