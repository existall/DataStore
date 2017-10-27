using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DapperExtensions;
using ExistsForAll.DataStore.Core;

namespace ExistsForAll.DataStore.DapperExtensions
{
	internal class DapperExtensionsQueryBuilder<T> : IQueryBuilder<T>
	{
		private PredicateGroup Predicates { get; } = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

		public IQueryBuilder<T> Where(Action<IConditionBuilder<T>> action)
		{
			Predicates.

			throw new NotImplementedException();
		}

		public IQueryBuilder<T> Take(int limit)
		{
			global::DapperExtensions.DapperExtensions.Update()

			Predicate<T>.Combine()
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