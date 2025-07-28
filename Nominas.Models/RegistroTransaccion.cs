using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nominas.Models;

public class RegistroTransaccion
{
    public int Id { get; set; }
    [Required]
    public int EmpleadoId { get; set; }
    public Empleado Empleado { get; set; }
    public decimal? Ingreso { get; set; }
    public decimal? Deduccion { get; set; }
    [DisplayName("Tipo de Transacción")]
    [Required]
    public TipoTransaccion TipoTransaccion { get; set; }
    [Required]
    public DateTime Fecha { get; set; }
    [Required]
    public decimal Monto { get; set; }
    [Required]
    public string Estado { get; set; }

}
