using System.ComponentModel.DataAnnotations.Schema;

public class Apuesta
{
    public int IdApuesta { get; set; }
    public int IdRuleta { get; set; }

    [ForeignKey(nameof(IdRuleta))]
    public Ruleta? Ruleta { get; set; }

    public string IdUsuario { get; set; } = null!; 
    public string TipoApuesta { get; set; } = null!; 
    public int? ValorNumero { get; set; }
    public string? ValorColor { get; set; }    
    public decimal Monto { get; set; }
    public string? Resultado { get; set; }    
    public decimal? Pago { get; set; }
}
