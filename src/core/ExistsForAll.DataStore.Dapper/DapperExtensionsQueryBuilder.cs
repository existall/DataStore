using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DapperExtensions;
using ExistsForAll.DataStore.Core;

namespace ExistsForAll.DataStore.DapperExtensions
{
	internal class DapperExtensionsQueryBuilder<T> : IQueryBuilder<T>
	{
		private IPredicate Predicate { get; }

		public IQueryBuilder<T> Where(Action<IConditionBuilder<T>> action)
		{
			//Predicates.

			//Predicate.
			throw new NotImplementedException();

		}

		public IQueryBuilder<T> Take(int limit)
		{
			throw new NotImplementedException();
		}

		public IQueryBuilder<T> Skip(int amount)
		{
			throw new NotImplementedException();
		}

		public IQueryBuilder<T> OrderBy(params Expression<Func<T, object>>[] fields)
		{
			throw new NotImplementedException();
		}

		public IQueryBuilder<T> OrderByDescending(params Expression<Func<T, object>>[] fields)
		{
			throw new NotImplementedException();
		}

		public IQueryBuilder<T> Select(params Expression<Func<T, object>>[] fields)
		{
			throw new NotImplementedException();
		}
	}
}