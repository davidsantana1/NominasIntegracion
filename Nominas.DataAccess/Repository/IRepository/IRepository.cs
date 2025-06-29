using System.Linq.Expressions;

namespace Nominas.DataAccess.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		IQueryable<T> GetAll();
		T Get(Expression<Func<T, bool>> filter);
		void Add(T entity);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entity);
	}
}
