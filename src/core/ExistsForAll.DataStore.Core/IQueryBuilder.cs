using System;
using System.Linq.Expressions;

namespace ExistsForAll.DataStore.Core
{
	public interface IQueryBuilder<T>
	{
		IQueryBuilder<T> Where(Action<IConditionBuilder<T>> action);

		IQueryBuilder<T> Take(int limit);

		IQueryBuilder<T> Skip(int amount);

		IQueryBuilder<T> OrderBy(params Expression<Func<T, object>>[] fields);

		IQueryBuilder<T> OrderByDescending(params Expression<Func<T, object>>[] fields);

		IQueryBuilder<T> Select(params Expression<Func<T, object>>[] fields);
	}
}
