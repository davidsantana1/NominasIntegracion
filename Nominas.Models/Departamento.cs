using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nominas.Models;

public class Departamento
{
	public int Id { get; set; }
	[Required]
	public string Nombre { get; set; }
	[DisplayName("Ubicación Física")]
	[Required]
	public string UbicacionFisica { get; set; }
	[DisplayName("Responsable del Área")]
	[Required]
	public int ResponsableDeAreaId { get; set; }
	public Empleado ResponsableDeArea { get; set; }
	public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
