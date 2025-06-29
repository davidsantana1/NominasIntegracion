using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nominas.DataAccess.Repository.IRepository;
using Nominas.Models;
using Nominas.Utility;

namespace NominasWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
	public class DepartamentoController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public DepartamentoController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			List<Departamento> objDepartamentoList = _unitOfWork.Departamento.GetAll().Include(e => e.Empleados).ToList();
			return View(objDepartamentoList);
		}

		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Crear()
		{
			var empleados = _unitOfWork.Empleado.GetAll();

			ViewBag.empleados = new SelectList(empleados, "Id", "Nombre");

			return View();
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Crear(Departamento obj)
		{
			obj.ResponsableDeArea = _unitOfWork.Empleado.Get(u => u.Id == obj.ResponsableDeAreaId);

			if (ModelState.IsValid)
			{
				_unitOfWork.Departamento.Add(obj);
				_unitOfWork.Save();
				TempData["success"] = "Departamento creado correctamente.";
				return RedirectToAction("Index");
			}
			return View();
		}

		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Editar(int? id)
		{
			var responsablesDeArea = _unitOfWork.Empleado.GetAll().ToList();

			ViewBag.empleados = new SelectList(responsablesDeArea, "Id", "Nombre");

			if (id == null || id == 0)
			{
				return NotFound();
			}
			Departamento? DepartamentoFromDb = _unitOfWork.Departamento.Get(u => u.Id == id);
			if (DepartamentoFromDb == null)
			{
				return NotFound();
			}
			return View(DepartamentoFromDb);
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Editar(Departamento obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Departamento.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "Departamento actualizado correctamente.";
				return RedirectToAction("Index");
			}
			return View();
		}


		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Eliminar(int? id)
		{
			var responsablesDeArea = _unitOfWork.Empleado.GetAll().ToList();

			ViewBag.empleados = new SelectList(responsablesDeArea, "Id", "Nombre");

			if (id == null || id == 0)
			{
				return NotFound();
			}
			Departamento? DepartamentoFromDb = _unitOfWork.Departamento.Get(u => u.Id == id);
			if (DepartamentoFromDb == null)
			{
				return NotFound();
			}
			return View(DepartamentoFromDb);
		}

		[HttpPost, ActionName("Eliminar")]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult EliminarPOST(int? id)
		{
			Departamento obj = _unitOfWork.Departamento.Get(u => u.Id == id);
			if (obj == null)
			{
				return NotFound();
			}
			_unitOfWork.Departamento.Remove(obj);
			_unitOfWork.Save();
			TempData["success"] = "Departamento eliminado correctamente.";
			return RedirectToAction("Index");
		}
	}
}

