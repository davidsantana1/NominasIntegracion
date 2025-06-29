using Nominas.DataAccess.Data;
using Nominas.DataAccess.Repository.IRepository;

namespace Nominas.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private ApplicationDbContext _db;
		public IDepartamentoRepository Departamento { get; private set; }
		public IEmpleadoRepository Empleado { get; private set; }
		public IPuestoRepository Puesto { get; private set; }
		public IRegistroTransaccionRepository RegistroTransaccion { get; private set; }
		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			Departamento = new DepartamentoRepository(_db);
			Empleado = new EmpleadoRepository(_db);
			Puesto = new PuestoRepository(_db);
			RegistroTransaccion = new RegistroTransaccionRepository(_db);
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
