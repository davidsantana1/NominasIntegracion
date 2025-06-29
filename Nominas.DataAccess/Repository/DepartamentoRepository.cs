using Nominas.DataAccess.Data;
using Nominas.DataAccess.Repository.IRepository;
using Nominas.Models;

namespace Nominas.DataAccess.Repository
{
	public class DepartamentoRepository : Repository<Departamento>, IDepartamentoRepository
	{
		private ApplicationDbContext _db;
		public DepartamentoRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Departamento obj)
		{
			_db.Departamentos.Update(obj);
		}
	}
}
