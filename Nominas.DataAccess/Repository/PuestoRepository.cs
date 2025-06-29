using Nominas.DataAccess.Data;
using Nominas.DataAccess.Repository.IRepository;
using Nominas.Models;

namespace Nominas.DataAccess.Repository
{
	public class PuestoRepository : Repository<Puesto>, IPuestoRepository
	{
		private ApplicationDbContext _db;

		public PuestoRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Puesto obj)
		{
			_db.Puestos.Update(obj);
		}
	}
}
