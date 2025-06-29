using Nominas.DataAccess.Data;
using Nominas.DataAccess.Repository.IRepository;
using Nominas.Models;

namespace Nominas.DataAccess.Repository
{
	public class RegistroTransaccionRepository : Repository<RegistroTransaccion>, IRegistroTransaccionRepository
	{
		private ApplicationDbContext _db;

		public RegistroTransaccionRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(RegistroTransaccion obj)
		{
			_db.RegistroTransacciones.Update(obj);
		}
	}
}
