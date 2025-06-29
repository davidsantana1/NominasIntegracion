using Nominas.DataAccess.Data;
using Nominas.DataAccess.Repository.IRepository;
using Nominas.Models;

namespace Nominas.DataAccess.Repository
{
	public class EmpleadoRepository : Repository<Empleado>, IEmpleadoRepository
	{
		private ApplicationDbContext _db;
		public EmpleadoRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Empleado obj)
		{
			_db.Empleados.Update(obj);
		}
	}
}
