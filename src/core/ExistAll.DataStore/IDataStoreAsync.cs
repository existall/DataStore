using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExistAll.DataStore
{
	public interface IDataStoreAsync<T> where T : class
	{
		Task<IEnumerable<T>> QueryAsync(Action<IQueryBuilder<T>> queryManipulator);
		Task<IEnumerable<T>> QueryAllAsync();

		Task<long> CountAsync(Action<IQueryBuilder<T>> queryManipulator = null);

		Task<T> GetByIdAsync(T id);
		Task AddAsync(T t);
		Task SaveAsync(T t);
		Task UpdateAsync(T t);
		Task DeleteAsync(T t);
	}
}
