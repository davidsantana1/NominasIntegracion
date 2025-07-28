using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nominas.DataAccess.Data;
using Nominas.DataAccess.Repository;
using Nominas.Utility;
using System.Text.Json;

namespace NominasWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class ReporteNominaController : Controller
    {
        private readonly NominaService _nominaService;
        private readonly ApplicationDbContext _db;
        private readonly IHttpClientFactory _httpClientFactory;

        public ReporteNominaController(NominaService nominaService, ApplicationDbContext db, IHttpClientFactory httpClientFactory)
        {
            _nominaService = nominaService;
            _db = db;
            _httpClientFactory = httpClientFactory;
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
                var reporteNomina = _nominaService.ObtenerNominasPorFecha(fechaDesde.Value, fechaHasta.Value);
                ViewBag.FechaDesde = fechaDesde.Value.ToString("yyyy-MM-dd");
                ViewBag.FechaHasta = fechaHasta.Value.ToString("yyyy-MM-dd");
                return View(reporteNomina);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
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
                    TempData["success"] = $"{nominasCreadas} nómina(s) nueva(s) ha(n) sido guardada(s).";
                }
                else
                {
                    TempData["info"] = "No se generaron nóminas nuevas. Ya todas estaban calculadas para el mes actual.";
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
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
                return Json(new { success = false, message = "Las fechas proporcionadas son inválidas." });
            }

            try
            {
                // 1. Obtener las nóminas del período
                var nominasDelPeriodo = _nominaService.ObtenerNominasPorFecha(fechas.FechaDesde, fechas.FechaHasta).ToList();
                if (!nominasDelPeriodo.Any())
                {
                    return Json(new { success = false, message = "No hay nóminas para contabilizar en el período seleccionado." });
                }

                // 2. Calcular el monto total y crear el objeto a enviar (sin cambios)
                var montoTotal = nominasDelPeriodo.Sum(n => n.SalarioNeto);
                var dataParaEnviar = new AsientoContableDto
                {
                    IdAuxiliar = 2,
                    Descripcion = $"Asiento de Nóminas correspondiente al período {fechas.FechaHasta:yyyy-MM-dd}",
                    CuentaDb = 71,
                    CuentaCr = 70,
                    MontoAsiento = montoTotal
                };

                // 3. Enviar a la API externa
                var httpClient = _httpClientFactory.CreateClient();
                string apiUrl = "URL_DE_TU_API_AQUI";

                HttpResponseMessage apiResponse = await httpClient.PostAsJsonAsync(apiUrl, dataParaEnviar);

                if (apiResponse.IsSuccessStatusCode)
                {
                    // 4. Leer y analizar la respuesta de la API
                    string responseBody = await apiResponse.Content.ReadAsStringAsync();
                    int idAsientoDesdeApi;

                    using (JsonDocument doc = JsonDocument.Parse(responseBody))
                    {
                        JsonElement root = doc.RootElement;

                        // Intenta obtener el ID. Abajo hay varios ejemplos.
                        // Descomenta el que se parezca más a la respuesta que esperas.

                        // Ejemplo 1: si la respuesta es {"id": 123} o {"asientoId": 123}
                        // idAsientoDesdeApi = root.GetProperty("id").GetInt32();

                        // Ejemplo 2: si la respuesta es {"data": {"id": 123}}
                        // idAsientoDesdeApi = root.GetProperty("data").GetProperty("id").GetInt32();

                        // Ejemplo 3: si la respuesta es solo el número "123"
                        // idAsientoDesdeApi = root.GetInt32();

                        // Por ahora, usaremos un valor de ejemplo. Reemplázalo con una de las líneas de arriba.
                        idAsientoDesdeApi = 999; // <--- VALOR DE PRUEBA, CAMBIAR LUEGO
                    }

                    foreach (var nomina in nominasDelPeriodo)
                    {
                        nomina.IdAsiento = idAsientoDesdeApi;
                    }

                    await _db.SaveChangesAsync();


                    return Json(new { success = true, message = $"Nóminas contabilizadas con el ID de Asiento: {idAsientoDesdeApi}." });
                }
                else
                {
                    string error = await apiResponse.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"Error de la API de contabilidad: {error}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error interno del servidor: {ex.InnerException?.Message ?? ex.Message}" });
            }
        }
    }
    public class FechasDto
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}