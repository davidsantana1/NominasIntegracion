using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nominas.Models;

public class Empleado
{
    public int Id { get; set; }

    [Required]
    [DisplayName("Cédula")]
    public string Cedula { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    [DisplayName("Departamento")]
    public int DepartamentoId { get; set; }
    public Departamento Departamento { get; set; }

    [Required]
    [DisplayName("Puesto")]
    public int PuestoId { get; set; }
    public Puesto Puesto { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El salario mensual debe ser mayor a 0")]
    [Required]
    [DisplayName("Salario Mensual")]
    public decimal SalarioMensual { get; set; }

    [Required]
    [DisplayName("Nómina")]
    public int NominaId { get; set; }
    public ICollection<RegistroTransaccion> RegistroTransacciones { get; set; }
}
