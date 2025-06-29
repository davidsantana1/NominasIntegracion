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
	public class EmpleadoController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public EmpleadoController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			List<Empleado> objEmpleadoList = _unitOfWork.Empleado.GetAll().Include(e => e.Departamento).Include(e => e.Puesto)
		.ToList();
			return View(objEmpleadoList);
		}
		public IActionResult Empleados(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}

			List<Empleado> objEmpleadoList = _unitOfWork.Empleado.GetAll()
				.Include(e => e.Departamento)
				.Include(e => e.Puesto)
				.Where(e => e.DepartamentoId == id)
				.ToList();
			var departamento = _unitOfWork.Departamento
			.Get(d => d.Id == id);

			if (departamento == null)
			{
				TempData["error"] = "El id del departamento es incorrecto.";
				return RedirectToAction("Index");
			}

			ViewData["Title"] = departamento != null ? $"Empleados de {departamento.Nombre}" : "Empleados";

			return View("Index", objEmpleadoList);
		}

		public IActionResult EmpleadosPorPuesto(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}

			List<Empleado> objEmpleadoList = _unitOfWork.Empleado.GetAll()
				.Include(e => e.Departamento)
				.Include(e => e.Puesto)
				.Where(e => e.PuestoId == id)
				.ToList();
			var puesto = _unitOfWork.Puesto
			.Get(d => d.Id == id);

			if (puesto == null)
			{
				TempData["error"] = "El id del puesto es incorrecto.";
				return RedirectToAction("Index");
			}

			ViewData["Title"] = puesto != null ? $"Empleados del puesto {puesto.Nombre}" : "Empleados";

			return View("Index", objEmpleadoList);
		}
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Crear()
		{
			var departamentos = _unitOfWork.Departamento.GetAll().ToList();
			var puestos = _unitOfWork.Puesto.GetAll().ToList();

			ViewBag.Departamentos = new SelectList(departamentos, "Id", "Nombre");
			ViewBag.Puestos = new SelectList(puestos, "Id", "Nombre");

			return View();
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Crear(Empleado obj)
		{
			obj.Departamento = _unitOfWork.Departamento.Get(d => d.Id == obj.DepartamentoId);
			obj.Puesto = _unitOfWork.Puesto.Get(p => p.Id == obj.PuestoId);

			if (ModelState.IsValid)
			{
				_unitOfWork.Empleado.Add(obj);
				_unitOfWork.Save();
				TempData["success"] = "Empleado creado correctamente.";
				return RedirectToAction("Index");
			}
			return View();
		}
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Editar(int? id)
		{
			var departamentos = _unitOfWork.Departamento.GetAll().ToList();
			var puestos = _unitOfWork.Puesto.GetAll().ToList();

			ViewBag.Departamentos = new SelectList(departamentos, "Id", "Nombre");
			ViewBag.Puestos = new SelectList(puestos, "Id", "Nombre");

			if (id == null || id == 0)
			{
				return NotFound();
			}
			Empleado? empleadoFromDb = _unitOfWork.Empleado.Get(e => e.Id == id);
			if (empleadoFromDb == null)
			{
				return NotFound();
			}
			return View(empleadoFromDb);
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Editar(Empleado obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Empleado.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "Empleado actualizado correctamente.";
				return RedirectToAction("Index");
			}
			return View();
		}
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Eliminar(int? id)
		{
			var departamentos = _unitOfWork.Departamento.GetAll().ToList();
			var puestos = _unitOfWork.Puesto.GetAll().ToList();

			ViewBag.Departamentos = new SelectList(departamentos, "Id", "Nombre");
			ViewBag.Puestos = new SelectList(puestos, "Id", "Nombre");

			if (id == null || id == 0)
			{
				return NotFound();
			}
			Empleado? empleadoFromDb = _unitOfWork.Empleado.Get(e => e.Id == id);
			if (empleadoFromDb == null)
			{
				return NotFound();
			}
			return View(empleadoFromDb);
		}

		[HttpPost, ActionName("Eliminar")]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult EliminarPOST(int? id)
		{
			Empleado obj = _unitOfWork.Empleado.Get(e => e.Id == id);
			if (obj == null)
			{
				return NotFound();
			}
			_unitOfWork.Empleado.Remove(obj);
			_unitOfWork.Save();
			TempData["success"] = "Empleado eliminado correctamente.";
			return RedirectToAction("Index");
		}
	}
}
