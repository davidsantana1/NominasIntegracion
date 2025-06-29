using Nominas.Models;

namespace Nominas.DataAccess.Repository.IRepository
{
	public interface IDepartamentoRepository : IRepository<Departamento>
	{
		void Update(Departamento obj);
	}
}
