using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nominas.Models;

public class TipoDeIngreso
{
    public int Id { get; set; }
    [Required]
    public string Nombre { get; set; }
    [DisplayName("Depende de Salario")]
    [Required]
    public bool DependeDeSalario { get; set; }
    [Required]
    public bool Estado { get; set; }
    public decimal Porcentaje { get; set; }
}
