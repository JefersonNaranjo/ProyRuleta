using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RuletaController : ControllerBase
{
    private readonly RuletaService _service;
    public RuletaController(RuletaService service) { _service = service; }

    [HttpPost("crear")]
    public async Task<IActionResult> Crear([FromBody] string nombre)
    {
        var id = await _service.CrearRuletaAsync(nombre);
        return Ok(new { IdRuleta = id });
    }

    [HttpPut("abrir/{idRuleta}")]
    public async Task<IActionResult> Abrir(int idRuleta)
    {
        var ok = await _service.AbrirRuletaAsync(idRuleta);
        if (!ok) return BadRequest(new { mensaje = "No se pudo abrir la ruleta." });
        return Ok(new { mensaje = "Ruleta abierta." });
    }

    [HttpPost("apostar")]
    public async Task<IActionResult> Apostar([FromBody] CrearApuestaDto dto)
    {
        if (!Request.Headers.TryGetValue("IdUsuario", out var headerUsuario))
            return BadRequest(new { mensaje = "Falta header IdUsuario" });

        if (string.IsNullOrWhiteSpace(dto.TipoApuesta))
            return BadRequest(new { mensaje = "TipoApuesta es obligatorio." });

        if (dto.TipoApuesta == "NUMERO" && !dto.ValorNumero.HasValue)
            return BadRequest(new { mensaje = "ValorNumero es obligatorio cuando TipoApuesta == NUMERO." });

        if (dto.TipoApuesta == "COLOR" && string.IsNullOrWhiteSpace(dto.ValorColor))
            return BadRequest(new { mensaje = "ValorColor es obligatorio cuando TipoApuesta == COLOR." });

        var idUsuario = headerUsuario.ToString();
        var result = await _service.RegistrarApuestaAsync(dto, idUsuario);

        if (!result.ok) return BadRequest(new { mensaje = result.mensaje });
        return Ok(new { mensaje = result.mensaje });
    }

    [HttpPost("cerrar/{idRuleta}")]
    public async Task<IActionResult> Cerrar(int idRuleta)
    {
        var resultado = await _service.CerrarRuletaAsync(idRuleta);
        if (resultado == null)
            return BadRequest(new { mensaje = "No se pudo cerrar la ruleta (verifique estado)." });
        return Ok(resultado);
    }
}
