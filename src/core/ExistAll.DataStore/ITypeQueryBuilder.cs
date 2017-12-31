using System;
using System.Linq.Expressions;

namespace ExistAll.DataStore
{
	public interface ITypeQueryBuilder<T>
	{
		ITypeQueryBuilder<T> Where(Action<ITypeConditionBuilder<T>> action);

		ITypeQueryBuilder<T> Take(int limit);

		ITypeQueryBuilder<T> Skip(int amount);

		ITypeQueryBuilder<T> OrderBy(params Expression<Func<T, object>>[] fields);

		ITypeQueryBuilder<T> OrderByDescending(params Expression<Func<T, object>>[] fields);

		ITypeQueryBuilder<T> Select(params Expression<Func<T, object>>[] fields);
	}
}