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
    public class RegistroTransaccionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistroTransaccionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(TipoTransaccion? tipoFiltro = null, int? empleadoId = null,
                         DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            IQueryable<RegistroTransaccion> query = _unitOfWork.RegistroTransaccion
                .GetAll()
                .Include(e => e.Empleado);

            if (tipoFiltro.HasValue)
                query = query.Where(t => t.TipoTransaccion == tipoFiltro.Value);

            if (empleadoId.HasValue)
                query = query.Where(t => t.EmpleadoId == empleadoId.Value);

            if (fechaInicio.HasValue)
                query = query.Where(t => t.Fecha >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(t => t.Fecha <= fechaFin.Value.AddDays(1)); // Incluir todo el día

            ViewBag.FiltroActivo = tipoFiltro;
            ViewBag.EmpleadoFiltroActivo = empleadoId;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;

            ViewBag.EmpleadosList = new SelectList(
                _unitOfWork.Empleado.GetAll(),
                "Id",
                "Nombre");

            return View(query.ToList());
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
        public IActionResult Crear(RegistroTransaccion obj)
        {
            obj.Empleado = _unitOfWork.Empleado.Get(u => u.Id == obj.EmpleadoId);

            if (ModelState.IsValid)
            {
                _unitOfWork.RegistroTransaccion.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Transacción creada correctamente.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Editar(int? id)
        {
            var empleados = _unitOfWork.Empleado.GetAll().ToList();

            ViewBag.empleados = new SelectList(empleados, "Id", "Nombre");

            if (id == null || id == 0)
            {
                return NotFound();
            }
            RegistroTransaccion? RegistroTransaccionFromDb = _unitOfWork.RegistroTransaccion.Get(u => u.Id == id);
            if (RegistroTransaccionFromDb == null)
            {
                return NotFound();
            }
            return View(RegistroTransaccionFromDb);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Editar(RegistroTransaccion obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.RegistroTransaccion.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Transacción actualizada correctamente.";
                return RedirectToAction("Index");
            }
            return View();
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Eliminar(int? id)
        {
            var empleados = _unitOfWork.Empleado.GetAll().ToList();

            ViewBag.empleados = new SelectList(empleados, "Id", "Nombre");

            if (id == null || id == 0)
            {
                return NotFound();
            }
            RegistroTransaccion? RegistroTransaccionFromDb = _unitOfWork.RegistroTransaccion.Get(u => u.Id == id);
            if (RegistroTransaccionFromDb == null)
            {
                return NotFound();
            }
            return View(RegistroTransaccionFromDb);
        }

        [HttpPost, ActionName("Eliminar")]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult EliminarPOST(int? id)
        {
            RegistroTransaccion obj = _unitOfWork.RegistroTransaccion.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.RegistroTransaccion.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Transacción eliminada correctamente.";
            return RedirectToAction("Index");
        }
    }
}
