using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RuletaService
{
    private readonly CasinoDbContext _db;
    private readonly Random _random = new Random();

    public RuletaService(CasinoDbContext db)
    {
        _db = db;
    }

    public async Task<int> CrearRuletaAsync(string nombre)
    {
        var ruleta = new Ruleta
        {
            Nombre = nombre,
            Estado = "CREADA"
        };

        _db.Ruletas.Add(ruleta);
        await _db.SaveChangesAsync();
        return ruleta.IdRuleta;
    }

    public async Task<bool> AbrirRuletaAsync(int idRuleta)
    {
        var ruleta = await _db.Ruletas.FindAsync(idRuleta);
        if (ruleta == null) return false;
        if (!string.Equals(ruleta.Estado, "CREADA", StringComparison.OrdinalIgnoreCase)) return false;

        ruleta.Estado = "ABIERTA";
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<(bool ok, string mensaje)> RegistrarApuestaAsync(CrearApuestaDto dto, string idUsuario)
    {
        var ruleta = await _db.Ruletas.FindAsync(dto.IdRuleta);
        if (ruleta == null || ruleta.Estado != "ABIERTA")
            return (false, "La ruleta no existe o no está abierta.");

        if (dto.Monto <= 0 || dto.Monto > 10000)
            return (false, "Monto inválido. Debe ser mayor que 0 y máximo 10000.");

        if (dto.TipoApuesta == "NUMERO")
        {
            if (!dto.ValorNumero.HasValue || dto.ValorNumero < 0 || dto.ValorNumero > 36)
                return (false, "Número inválido (debe ser entre 0 y 36).");
        }
        else if (dto.TipoApuesta == "COLOR")
        {
            if (dto.ValorColor != "ROJO" && dto.ValorColor != "NEGRO")
                return (false, "Color inválido (solo ROJO o NEGRO).");
        }
        else
        {
            return (false, "Tipo de apuesta inválido (use NUMERO o COLOR).");
        }

        var apuesta = new Apuesta
        {
            IdRuleta = dto.IdRuleta,
            IdUsuario = idUsuario,
            TipoApuesta = dto.TipoApuesta,
            ValorNumero = dto.ValorNumero,
            ValorColor = dto.ValorColor,
            Monto = dto.Monto
        };

        _db.Apuestas.Add(apuesta);
        await _db.SaveChangesAsync();

        return (true, "Apuesta registrada correctamente.");
    }

    public async Task<ResultadoCierreDto> CerrarRuletaAsync(int idRuleta)
    {
        var ruleta = await _db.Ruletas.Include(r => r.Apuestas)
            .FirstOrDefaultAsync(r => r.IdRuleta == idRuleta);

        if (ruleta == null || ruleta.Estado != "ABIERTA")
            return null;

        int numeroGanador = _random.Next(0, 37);
        string colorGanador = (numeroGanador % 2 == 0) ? "ROJO" : "NEGRO";

        ruleta.NumeroGanador = numeroGanador;
        ruleta.ColorGanador = colorGanador;
        ruleta.Estado = "CERRADA";

        var listaResultados = new List<object>();

        foreach (var ap in ruleta.Apuestas)
        {
            if (ap.TipoApuesta == "NUMERO" && ap.ValorNumero == numeroGanador)
            {
                ap.Resultado = "GANADA";
                ap.Pago = ap.Monto * 5m;
            }
            else if (ap.TipoApuesta == "COLOR" && ap.ValorColor == colorGanador)
            {
                ap.Resultado = "GANADA";
                ap.Pago = ap.Monto * 1.8m;
            }
            else
            {
                ap.Resultado = "PERDIDA";
                ap.Pago = 0m;
            }

            listaResultados.Add(new
            {
                ap.IdApuesta,
                ap.IdUsuario,
                ap.TipoApuesta,
                ap.ValorNumero,
                ap.ValorColor,
                ap.Monto,
                ap.Resultado,
                ap.Pago
            });
        }

        await _db.SaveChangesAsync();

        return new ResultadoCierreDto
        {
            IdRuleta = ruleta.IdRuleta,
            NumeroGanador = numeroGanador,
            ColorGanador = colorGanador,
            ApuestasProcesadas = listaResultados
        };
    }
}
