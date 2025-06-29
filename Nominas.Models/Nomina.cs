using System.ComponentModel.DataAnnotations.Schema;

namespace Nominas.Models
{
    public class Nomina
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal SalarioNeto { get; set; }
        public DateTime FechaGeneracion { get; set; }

        [NotMapped]
        public List<DetalleDeduccion> DetallesDeducciones { get; set; } = new List<DetalleDeduccion>();
        [NotMapped]
        public List<DetalleIngreso> DetallesIngresos { get; set; } = new List<DetalleIngreso>();
    }

}

public class DetalleDeduccion
{
    public string Nombre { get; set; }
    public decimal Monto { get; set; }
}

public class DetalleIngreso
{
    public string Nombre { get; set; }
    public decimal Monto { get; set; }
}