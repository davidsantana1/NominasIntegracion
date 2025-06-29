using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nominas.DataAccess.Data;
using Nominas.Models;
using Nominas.Utility;

namespace NominasWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
	public class TipoDeIngresoController : Controller
	{
		private readonly ApplicationDbContext _db;
		public TipoDeIngresoController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			List<TipoDeIngreso> objPuestoList = _db.TiposDeIngreso.ToList();
			return View(objPuestoList);
		}
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Crear()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Crear(TipoDeIngreso obj)
		{
			if (ModelState.IsValid)
			{
				_db.TiposDeIngreso.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Tipo de Ingreso creado correctamente.";
				return RedirectToAction("Index");
			}
			return View();
		}
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Editar(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			TipoDeIngreso? tipoDeIngresoFromDb = _db.TiposDeIngreso.Find(id);
			if (tipoDeIngresoFromDb == null)
			{
				TempData["error"] = "El id del Tipo de Ingreso es incorrecto.";
				return RedirectToAction("Index");
			}
			return View(tipoDeIngresoFromDb);
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Editar(TipoDeIngreso obj)
		{
			if (ModelState.IsValid)
			{
				_db.TiposDeIngreso.Update(obj);
				_db.SaveChanges();
				TempData["success"] = "Tipo de Ingreso actualizado correctamente.";
				return RedirectToAction("Index");
			}

			return View();
		}
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Eliminar(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			TipoDeIngreso? tipoDeIngresoFromDb = _db.TiposDeIngreso.Find(id);
			if (tipoDeIngresoFromDb == null)
			{
				return NotFound();
			}
			return View(tipoDeIngresoFromDb);
		}

		[HttpPost, ActionName("Eliminar")]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult EliminarPOST(int? id)
		{
			TipoDeIngreso? obj = _db.TiposDeIngreso.Find(id);
			if (obj == null)
			{
				return NotFound();
			}
			_db.TiposDeIngreso.Remove(obj);
			_db.SaveChanges();
			TempData["success"] = "Tipo de Ingreso eliminado correctamente.";
			return RedirectToAction("Index");
		}
	}
}
