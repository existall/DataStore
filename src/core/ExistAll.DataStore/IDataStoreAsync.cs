using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExistAll.DataStore
{
	public interface IDataStoreAsync<T> where T : class
	{
		Task<IEnumerable<T>> QueryAsync(Action<IQueryBuilder<T>> queryManipulator);
		Task<IEnumerable<T>> QueryAllAsync();

		Task<long> CountAsync(Action<IConditionBuilder<T>> conditionManipulator = null);

		Task<T> GetByIdAsync(T id);
		Task AddAsync(T t);
		Task SaveAsync(T t);
		Task UpdateAsync(T t);
		Task DeleteAsync(T t);
	}
}
