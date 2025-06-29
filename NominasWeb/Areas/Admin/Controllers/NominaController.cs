using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nominas.DataAccess.Repository;
using Nominas.DataAccess.Repository.IRepository;
using Nominas.Models;
using Nominas.Utility;

namespace NominasWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
	public class NominaController : Controller
	{
		private readonly NominaService _nominaService;
		private readonly IRepository<Empleado> _empleadoRepository; // Agregado

		public NominaController(NominaService nominaService, IRepository<Empleado> empleadoRepository)
		{
			_nominaService = nominaService;
			_empleadoRepository = empleadoRepository; // Inicializado
		}

		public IActionResult Index()
		{
			// Obtén la lista de empleados para el dropdown
			var empleados = _empleadoRepository.GetAll().ToList();
			var viewModel = new NominaViewModel
			{
				Empleados = empleados
			};

			return View(viewModel);
		}

		public IActionResult Calcular(int empleadoId)
		{
			try
			{
				var nomina = _nominaService.CalcularNominaEmpleado(empleadoId);

				return View("NominaDetails", nomina); // Asegúrate de que la vista sea correcta
			}
			catch (Exception ex)
			{
				// Manejar la excepción de manera adecuada
				ViewBag.ErrorMessage = ex.Message;
				return View("Error");
			}
		}
	}
}
