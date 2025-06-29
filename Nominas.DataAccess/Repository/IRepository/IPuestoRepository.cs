using Nominas.Models;

namespace Nominas.DataAccess.Repository.IRepository
{
	public interface IPuestoRepository : IRepository<Puesto>
	{
		void Update(Puesto obj);
	}
}
