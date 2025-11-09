using System.Collections.Generic;

public class ResultadoCierreDto
{
    public int IdRuleta { get; set; }
    public int NumeroGanador { get; set; }
    public string ColorGanador { get; set; }
    public List<object> ApuestasProcesadas { get; set; }
}
