namespace Nominas.Models.DTOs
{
    public class AccountingApiConfig
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public Endpoints Endpoints { get; set; } = new Endpoints();
    }

    public class Endpoints
    {
        public string EntradasContables { get; set; } = string.Empty;
    }
}
