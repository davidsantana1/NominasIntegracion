using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nominas.DataAccess.Data;
using Nominas.Models;
using Nominas.Utility;

namespace NominasWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
	public class TipoDeDeduccionController : Controller
	{
		private readonly ApplicationDbContext _db;
		public TipoDeDeduccionController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			List<TipoDeDeduccion> objPuestoList = _db.TiposDeDeducciones.ToList();
			return View(objPuestoList);
		}
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Crear()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Crear(TipoDeDeduccion obj)
		{
			if (ModelState.IsValid)
			{
				_db.TiposDeDeducciones.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Tipo de Deduccion creado correctamente.";
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
			TipoDeDeduccion? tipoDeDeduccionFromDb = _db.TiposDeDeducciones.Find(id);
			if (tipoDeDeduccionFromDb == null)
			{
				TempData["error"] = "El id del Tipo de Deduccion es incorrecto.";
				return RedirectToAction("Index");
			}
			return View(tipoDeDeduccionFromDb);
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Editar(TipoDeDeduccion obj)
		{
			if (ModelState.IsValid)
			{
				_db.TiposDeDeducciones.Update(obj);
				_db.SaveChanges();
				TempData["success"] = "Tipo de Deduccion actualizado correctamente.";
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
			TipoDeDeduccion? tipoDeDeduccionFromDb = _db.TiposDeDeducciones.Find(id);
			if (tipoDeDeduccionFromDb == null)
			{
				return NotFound();
			}
			return View(tipoDeDeduccionFromDb);
		}

		[HttpPost, ActionName("Eliminar")]
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult EliminarPOST(int? id)
		{
			TipoDeDeduccion? obj = _db.TiposDeDeducciones.Find(id);
			if (obj == null)
			{
				return NotFound();
			}
			_db.TiposDeDeducciones.Remove(obj);
			_db.SaveChanges();
			TempData["success"] = "Tipo de Deduccion eliminado correctamente.";
			return RedirectToAction("Index");
		}
	}
}
