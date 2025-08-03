using System.Text.Json.Serialization;

public class AsientoContableDto
{
    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; }

    [JsonPropertyName("cuenta_Id")]
    public int CuentaId { get; set; }

    [JsonPropertyName("auxiliar_Id")]
    public int AuxiliarId { get; set; }

    [JsonPropertyName("tipoMovimiento")]
    public string TipoMovimiento { get; set; }

    [JsonPropertyName("fechaAsiento")]
    public string FechaAsiento { get; set; }

    [JsonPropertyName("montoAsiento")]
    public decimal MontoAsiento { get; set; }
}
