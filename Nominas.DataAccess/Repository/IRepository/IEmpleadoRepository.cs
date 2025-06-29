using Nominas.Models;

namespace Nominas.DataAccess.Repository.IRepository
{
	public interface IEmpleadoRepository : IRepository<Empleado>
	{
		void Update(Empleado obj);
	}
}
