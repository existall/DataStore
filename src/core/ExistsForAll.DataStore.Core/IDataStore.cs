using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExistsForAll.DataStore.Core
{
	public interface IDataStore<T> where T : class
	{
		Task<List<T>> QueryAsync(Action<IQueryBuilder<T>> queryManipulator);
		Task<List<T>> QueryAllAsync();

		Task<long> CountAsync(Action<IQueryBuilder<T>> queryManipulator = null);

		Task<T> GetByIdAsync(T id);
		Task AddAsync(T t);
		Task SaveAsync(T t);
		Task UpdateAsync(T t);
		Task DeleteAsync(T t);

		Task<List<T>> GetByFieldAsync<TKey>(Expression<Func<T, TKey>> selector, TKey key);
		Task<List<T>> GetByFieldAsync<TKey>(Expression<Func<T, TKey>> selector, IEnumerable<TKey> ids);
	}
}