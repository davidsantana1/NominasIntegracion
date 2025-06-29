using Nominas.Models;

namespace Nominas.DataAccess.Repository.IRepository
{
	public interface IRegistroTransaccionRepository : IRepository<RegistroTransaccion>
	{
		void Update(RegistroTransaccion obj);
	}
}
