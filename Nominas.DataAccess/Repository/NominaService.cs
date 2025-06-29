using Nominas.DataAccess.Repository.IRepository;
using Nominas.Models;

namespace Nominas.DataAccess.Repository
{
	public class NominaService
	{
		private readonly IRepository<Empleado> _empleadoRepository;
		private readonly IRepository<TipoDeDeduccion> _deduccionRepository;
		private readonly IRepository<TipoDeIngreso> _ingresoRepository;

		public NominaService(IRepository<Empleado> empleadoRepository,
							 IRepository<TipoDeDeduccion> deduccionRepository,
							 IRepository<TipoDeIngreso> ingresoRepository)
		{
			_empleadoRepository = empleadoRepository;
			_deduccionRepository = deduccionRepository;
			_ingresoRepository = ingresoRepository;
		}

		public Nomina CalcularNominaEmpleado(int empleadoId)
		{
			var empleado = _empleadoRepository.Get(u => u.Id == empleadoId);
			if (empleado == null) throw new Exception("Empleado no encontrado");

			var salarioBruto = empleado.SalarioMensual;

			var deducciones = _deduccionRepository.GetAll()
				.Where(t => t.Estado && t.DependeDeSalario)
				.ToList();

			var ingresos = _ingresoRepository.GetAll()
				.Where(t => t.Estado && t.DependeDeSalario)
				.ToList();

			var detallesDeducciones = deducciones.Select(t => new DetalleDeduccion
			{
				Nombre = t.Nombre,
				Monto = salarioBruto * ObtenerPorcentajeDeduccion(t.Id)
			}).ToList();

			var detallesIngresos = ingresos.Select(t => new DetalleIngreso
			{
				Nombre = t.Nombre,
				Monto = salarioBruto * ObtenerPorcentajeIngreso(t.Id)
			}).ToList();

			var deduccionesTotales = detallesDeducciones.Sum(d => d.Monto);
			var ingresosTotales = detallesIngresos.Sum(i => i.Monto);
			var salarioNeto = salarioBruto + ingresosTotales - deduccionesTotales;

			return new Nomina
			{
				Empleado = empleado,
				EmpleadoId = empleadoId,
				SalarioBruto = salarioBruto,
				TotalDeducciones = deduccionesTotales,
				TotalIngresos = ingresosTotales,
				SalarioNeto = salarioNeto,
				FechaGeneracion = DateTime.Now,
				DetallesDeducciones = detallesDeducciones,
				DetallesIngresos = detallesIngresos
			};
		}

		public IEnumerable<Nomina> CalcularNominaTotal()
		{
			var empleados = _empleadoRepository.GetAll().ToList();
			var deducciones = _deduccionRepository.GetAll()
				.Where(t => t.Estado && t.DependeDeSalario)
				.ToList();

			var ingresos = _ingresoRepository.GetAll()
				.Where(t => t.Estado && t.DependeDeSalario)
				.ToList();
			var todasLasNominas = new List<Nomina>();

			foreach (var empleado in empleados)
			{
				var salarioBruto = empleado.SalarioMensual;

				var detallesDeducciones = deducciones.Select(t => new DetalleDeduccion
				{
					Nombre = t.Nombre,
					Monto = salarioBruto * ObtenerPorcentajeDeduccion(t.Id)
				}).ToList();

				var detallesIngresos = ingresos.Select(t => new DetalleIngreso
				{
					Nombre = t.Nombre,
					Monto = salarioBruto * ObtenerPorcentajeIngreso(t.Id)
				}).ToList();

				var deduccionesTotales = detallesDeducciones.Sum(d => d.Monto);
				var ingresosTotales = detallesIngresos.Sum(i => i.Monto);
				var salarioNeto = salarioBruto + ingresosTotales - deduccionesTotales;

				todasLasNominas.Add(new Nomina
				{
					Empleado = empleado,
					EmpleadoId = empleado.Id,
					SalarioBruto = salarioBruto,
					TotalDeducciones = deduccionesTotales,
					TotalIngresos = ingresosTotales,
					SalarioNeto = salarioNeto,
					FechaGeneracion = DateTime.Now,
					DetallesDeducciones = detallesDeducciones,
					DetallesIngresos = detallesIngresos
				});
			}

			return todasLasNominas;
		}


		public IEnumerable<Empleado> ObtenerEmpleados()
		{
			return _empleadoRepository.GetAll().ToList();
		}


		private decimal ObtenerPorcentajeDeduccion(int deduccionId)
		{
			var deduccion = _deduccionRepository.Get(d => d.Id == deduccionId);
			if (deduccion == null) throw new Exception("Tipo de deducción no encontrado");

			return deduccion.Porcentaje / 100;
		}

		private decimal ObtenerPorcentajeIngreso(int ingresoId)
		{
			var ingreso = _ingresoRepository.Get(i => i.Id == ingresoId);
			if (ingreso == null) throw new Exception("Tipo de ingreso no encontrado");

			return ingreso.Porcentaje / 100;
		}
	}




}
