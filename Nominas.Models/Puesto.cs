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
    [Range(0.01, double.MaxValue, ErrorMessage = "El salario mínimo debe ser mayor a 0")]
    public decimal NivelMinimoSalario { get; set; }

    [DisplayName("Nivel Máximo Salario")]
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El salario máximo debe ser mayor a 0")]
    public decimal NivelMaximoSalario { get; set; }
}
