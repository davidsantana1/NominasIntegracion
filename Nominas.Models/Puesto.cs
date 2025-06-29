using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nominas.Models;

public class Puesto
{
    public int Id { get; set; }
    [Required]
    public string Nombre { get; set; }
    [DisplayName("Nivel de Riesgo")]
    [Required]
    public string NivelDeRiesgo { get; set; }
    [DisplayName("Nivel Mínimo Salario")]
    [Required]
    public decimal NivelMinimoSalario { get; set; }
    [DisplayName("Nivel Máximo Salario")]
    [Required]
    public decimal NivelMaximoSalario { get; set; }
}
