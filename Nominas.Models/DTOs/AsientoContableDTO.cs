using System.Text.Json.Serialization;

public class AsientoContableDto
{
    [JsonPropertyName("id_auxiliar")]
    public int IdAuxiliar { get; set; }

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; }

    [JsonPropertyName("cuenta_db")]
    public int CuentaDb { get; set; }

    [JsonPropertyName("cuenta_cr")]
    public int CuentaCr { get; set; }

    [JsonPropertyName("monto_asiento")]
    public decimal MontoAsiento { get; set; }
}