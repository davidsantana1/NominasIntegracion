using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nominas.DataAccess.Repository;
using Nominas.Utility;

namespace NominasWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
	public class ReporteNominaController : Controller
	{
		private readonly NominaService _nominaService;

		public ReporteNominaController(NominaService nominaService)
		{
			_nominaService = nominaService;
		}

		public IActionResult Index()
		{
			try
			{
				var reporteNomina = _nominaService.CalcularNominaTotal();
				return View(reporteNomina);
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View("Error");
			}
		}
	}
}
