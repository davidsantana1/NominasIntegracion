using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nominas.Models;

public class RegistroTransaccion
{
    public int Id { get; set; }

    [Required]
    public int EmpleadoId { get; set; }
    public Empleado Empleado { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El ingreso debe ser mayor a 0")]
    public decimal? Ingreso { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "La deducción debe ser mayor a 0")]
    public decimal? Deduccion { get; set; }

    [DisplayName("Tipo de Transacción")]
    [Required]
    public TipoTransaccion TipoTransaccion { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Monto { get; set; }

    [Required]
    public string Estado { get; set; }
}
