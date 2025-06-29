public class ReporteNominaViewModel
{
    public DateTime FechaGeneracion { get; set; }
    public List<DetalleNomina> DetallesNomina { get; set; }
}

public class DetalleNomina
{
    public string EmpleadoNombre { get; set; }
    public decimal SalarioBruto { get; set; }
    public decimal TotalDeducciones { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal SalarioNeto { get; set; }
}
