namespace Nominas.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		IDepartamentoRepository Departamento { get; }
		IEmpleadoRepository Empleado { get; }
		IPuestoRepository Puesto { get; }
		IRegistroTransaccionRepository RegistroTransaccion { get; }
		void Save();
	}
}
