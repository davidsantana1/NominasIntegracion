using Microsoft.EntityFrameworkCore;
using Nominas.DataAccess.Data;
using Nominas.DataAccess.Repository.IRepository;
using Nominas.Models;

namespace Nominas.DataAccess.Repository
{
    public class NominaService
    {
        private readonly IRepository<Empleado> _empleadoRepository;
        private readonly IRepository<TipoDeDeduccion> _deduccionRepository;
        private readonly IRepository<TipoDeIngreso> _ingresoRepository;
        private readonly IRepository<Nomina> _nominaRepository;
        private readonly ApplicationDbContext _db;

        public NominaService(
            IRepository<Empleado> empleadoRepository,
            IRepository<TipoDeDeduccion> deduccionRepository,
            IRepository<TipoDeIngreso> ingresoRepository,
            IRepository<Nomina> nominaRepository,
            ApplicationDbContext db)
        {
            _empleadoRepository = empleadoRepository;
            _deduccionRepository = deduccionRepository;
            _ingresoRepository = ingresoRepository;
            _nominaRepository = nominaRepository;
            _db = db;
        }

        public IEnumerable<Nomina> ObtenerNominasPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            var fechaHastaAjustada = fechaHasta.Date.AddDays(1).AddTicks(-1);

            var nominasFiltradas = _db.Nominas
                .Include(n => n.Empleado)
                .Where(n => n.FechaGeneracion >= fechaDesde.Date && n.FechaGeneracion <= fechaHastaAjustada)
                .ToList();

            return nominasFiltradas;
        }

        public int GenerarYGuardarNominaMensual()
        {
            var empleados = _empleadoRepository.GetAll().ToList();
            var nominasNuevasCreadas = 0;
            var mesActual = DateTime.Now.Month;
            var anioActual = DateTime.Now.Year;

            var nominasDelMesExistentes = _nominaRepository.GetAll()
                .Where(n => n.FechaGeneracion.Month == mesActual && n.FechaGeneracion.Year == anioActual)
                .Select(n => n.EmpleadoId)
                .ToHashSet();

            foreach (var empleado in empleados)
            {
                if (!nominasDelMesExistentes.Contains(empleado.Id))
                {
                    var nuevaNomina = CalcularNominaEmpleado(empleado.Id);
                    _nominaRepository.Add(nuevaNomina);
                    nominasNuevasCreadas++;
                }
            }

            if (nominasNuevasCreadas > 0)
            {
                _db.SaveChanges();
            }

            return nominasNuevasCreadas;
        }

        public Nomina CalcularNominaEmpleado(int empleadoId)
        {
            var empleado = _empleadoRepository.Get(u => u.Id == empleadoId);
            if (empleado == null) throw new Exception("Empleado no encontrado");

            var salarioBruto = empleado.SalarioMensual;
            var deducciones = _deduccionRepository.GetAll().Where(t => t.Estado && t.DependeDeSalario).ToList();
            var ingresos = _ingresoRepository.GetAll().Where(t => t.Estado && t.DependeDeSalario).ToList();
            var detallesDeducciones = deducciones.Select(t => new DetalleDeduccion { Nombre = t.Nombre, Monto = salarioBruto * (t.Porcentaje / 100) }).ToList();
            var detallesIngresos = ingresos.Select(t => new DetalleIngreso { Nombre = t.Nombre, Monto = salarioBruto * (t.Porcentaje / 100) }).ToList();
            var deduccionesTotales = detallesDeducciones.Sum(d => d.Monto);
            var ingresosTotales = detallesIngresos.Sum(i => i.Monto);
            var salarioNeto = salarioBruto + ingresosTotales - deduccionesTotales;

            return new Nomina
            {
                EmpleadoId = empleadoId,
                SalarioBruto = salarioBruto,
                TotalDeducciones = deduccionesTotales,
                TotalIngresos = ingresosTotales,
                SalarioNeto = salarioNeto,
                FechaGeneracion = DateTime.Now,
                IdAsiento = null,
                Empleado = empleado
            };
        }
    }
}