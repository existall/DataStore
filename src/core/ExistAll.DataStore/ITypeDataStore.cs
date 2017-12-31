using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExistAll.DataStore
{
	public interface ITypeDataStore<T> where T : class
	{
		Task<List<T>> QueryAsync(Action<ITypeQueryBuilder<T>> queryManipulator);
		Task<List<T>> QueryAllAsync();

		Task<long> CountAsync(Action<ITypeQueryBuilder<T>> queryManipulator = null);

		Task<T> GetByIdAsync(T id);
		Task AddAsync(T t);
		Task SaveAsync(T t);
		Task UpdateAsync(T t);
		Task DeleteAsync(T t);

		Task<List<T>> GetByFieldAsync(Expression<Func<T, object>> selector, object key);
		Task<List<T>> GetByFieldAsync(Expression<Func<T, object>> selector, IEnumerable<object> ids);
	}
}