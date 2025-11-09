using System.Collections.Generic;

public class Ruleta
{
    public int IdRuleta { get; set; }
    public string? Nombre { get; set; }          
    public string Estado { get; set; }         
    public int? NumeroGanador { get; set; }
    public string? ColorGanador { get; set; }    
    public ICollection<Apuesta>? Apuestas { get; set; }
}
