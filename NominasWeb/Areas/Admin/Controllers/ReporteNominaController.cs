using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nominas.DataAccess.Data;
using Nominas.DataAccess.Repository;
using Nominas.Models.DTOs;
using Nominas.Utility;

namespace NominasWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class ReporteNominaController : Controller
    {
        private readonly NominaService _nominaService;
        private readonly ApplicationDbContext _db;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ReporteNominaController(
            NominaService nominaService,
            ApplicationDbContext db,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration
        )
        {
            _nominaService = nominaService;
            _db = db;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public IActionResult Index(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            if (!fechaDesde.HasValue)
            {
                fechaDesde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            if (!fechaHasta.HasValue)
            {
                fechaHasta = fechaDesde.Value.AddMonths(1).AddDays(-1);
            }

            try
            {
                var reporteNomina = _nominaService.ObtenerNominasPorFecha(
                    fechaDesde.Value,
                    fechaHasta.Value
                );
                ViewBag.FechaDesde = fechaDesde.Value.ToString("yyyy-MM-dd");
                ViewBag.FechaHasta = fechaHasta.Value.ToString("yyyy-MM-dd");
                return View(reporteNomina);
            }
            catch (Exception ex)
            {
                var errorMessage =
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.ErrorMessage = errorMessage;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerarYGuardarNominas()
        {
            try
            {
                var nominasCreadas = _nominaService.GenerarYGuardarNominaMensual();

                if (nominasCreadas > 0)
                {
                    _db.SaveChanges();
                    TempData["success"] =
                        $"{nominasCreadas} nómina(s) nueva(s) ha(n) sido guardada(s).";
                }
                else
                {
                    TempData["info"] =
                        "No se generaron nóminas nuevas. Ya todas estaban calculadas para el mes actual.";
                }
            }
            catch (Exception ex)
            {
                var errorMessage =
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["error"] = $"Error al generar las nóminas: {errorMessage}";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contabilizar([FromBody] FechasDto fechas)
        {
            if (fechas == null || fechas.FechaDesde == default || fechas.FechaHasta == default)
            {
                return Json(
                    new { success = false, message = "Las fechas proporcionadas son inválidas." }
                );
            }

            try
            {
                // 1. Obtener las nóminas del período que NO tienen ID de asiento asignado
                var nominasDelPeriodo = _nominaService
                    .ObtenerNominasPorFecha(fechas.FechaDesde, fechas.FechaHasta)
                    .Where(n => n.IdAsiento == null || n.IdAsiento == 0) // Solo nóminas sin asiento asignado
                    .ToList();

                if (!nominasDelPeriodo.Any())
                {
                    return Json(
                        new
                        {
                            success = false,
                            message = "No hay nóminas pendientes de contabilizar en el período seleccionado. Todas ya han sido contabilizadas.",
                        }
                    );
                }

                // 2. Calcular el monto total y crear los objetos a enviar según la especificación de la API
                var montoTotal = nominasDelPeriodo.Sum(n => n.SalarioNeto);

                // Crear el asiento de débito (cuenta 70)
                var asientoDebito = new AsientoContableDto
                {
                    Descripcion =
                        $"Asiento de Nóminas correspondiente al período {fechas.FechaHasta:yyyy-MM-dd}",
                    CuentaId = 70,
                    AuxiliarId = 1, // Valor por defecto
                    TipoMovimiento = "DB", // Débito
                    FechaAsiento = fechas.FechaHasta.ToString("yyyy-MM-dd"),
                    MontoAsiento = montoTotal,
                };

                // Crear el asiento de crédito (cuenta 71)
                var asientoCredito = new AsientoContableDto
                {
                    Descripcion =
                        $"Asiento de Nóminas correspondiente al período {fechas.FechaHasta:yyyy-MM-dd}",
                    CuentaId = 71,
                    AuxiliarId = 1, // Valor por defecto
                    TipoMovimiento = "CR", // Crédito
                    FechaAsiento = fechas.FechaHasta.ToString("yyyy-MM-dd"),
                    MontoAsiento = montoTotal,
                };

                // 3. Enviar a la API externa
                var httpClient = _httpClientFactory.CreateClient();

                // Obtener configuración de la API
                var apiConfig = new AccountingApiConfig();
                _configuration.GetSection("AccountingApi").Bind(apiConfig);

                string apiUrl = $"{apiConfig.BaseUrl}{apiConfig.Endpoints.EntradasContables}";

                // Configurar headers con autenticación
                httpClient.DefaultRequestHeaders.Add("x-api-key", apiConfig.ApiKey);

                // 3. Enviar ambos asientos a la API externa
                bool ambosExitosos = true;
                string errorMessage = "";

                // Enviar asiento de débito
                var jsonDebito = System.Text.Json.JsonSerializer.Serialize(asientoDebito);
                Console.WriteLine($"Enviando asiento débito: {jsonDebito}");

                HttpResponseMessage apiResponseDebito = await httpClient.PostAsJsonAsync(
                    apiUrl,
                    asientoDebito
                );
                if (!apiResponseDebito.IsSuccessStatusCode)
                {
                    string errorDebito = await apiResponseDebito.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error en asiento débito: {errorDebito}");
                    errorMessage += $"Error en asiento débito: {errorDebito}. ";
                    ambosExitosos = false;
                }

                // Enviar asiento de crédito
                var jsonCredito = System.Text.Json.JsonSerializer.Serialize(asientoCredito);
                Console.WriteLine($"Enviando asiento crédito: {jsonCredito}");

                HttpResponseMessage apiResponseCredito = await httpClient.PostAsJsonAsync(
                    apiUrl,
                    asientoCredito
                );
                if (!apiResponseCredito.IsSuccessStatusCode)
                {
                    string errorCredito = await apiResponseCredito.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error en asiento crédito: {errorCredito}");
                    errorMessage += $"Error en asiento crédito: {errorCredito}. ";
                    ambosExitosos = false;
                }

                if (ambosExitosos)
                {
                    // Leer la respuesta del asiento de débito para obtener el ID
                    string responseBodyDebito = await apiResponseDebito.Content.ReadAsStringAsync();
                    int idAsientoDesdeApi = 999; // Valor por defecto

                    try
                    {
                        using (JsonDocument doc = JsonDocument.Parse(responseBodyDebito))
                        {
                            JsonElement root = doc.RootElement;

                            // Intentar obtener el ID del asiento desde la respuesta
                            if (
                                root.TryGetProperty("data", out JsonElement dataElement)
                                && dataElement.TryGetProperty("id", out JsonElement idElement)
                            )
                            {
                                idAsientoDesdeApi = idElement.GetInt32();
                            }
                            else if (root.TryGetProperty("id", out JsonElement directIdElement))
                            {
                                idAsientoDesdeApi = directIdElement.GetInt32();
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine(
                            $"Error al procesar la respuesta del asiento débito: {ex.Message}"
                        );
                        // Continuar con el ID por defecto
                    }

                    // Actualizar las nóminas con el ID del asiento
                    foreach (var nomina in nominasDelPeriodo)
                    {
                        nomina.IdAsiento = idAsientoDesdeApi;
                    }

                    await _db.SaveChangesAsync();

                    return Json(
                        new
                        {
                            success = true,
                            message = $"Nóminas contabilizadas exitosamente. ID de Asiento: {idAsientoDesdeApi}",
                        }
                    );
                }
                else
                {
                    return Json(
                        new
                        {
                            success = false,
                            message = $"Error de la API de contabilidad: {errorMessage}",
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = $"Error interno del servidor: {ex.InnerException?.Message ?? ex.Message}",
                    }
                );
            }
        }
    }

    public class FechasDto
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}
