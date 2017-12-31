using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExistAll.DataStore;
using ExistsForAll.DapperExtensions;
using ExistsForAll.DapperExtensions.Predicates;

namespace ExistsForAll.DataStore.DapperExtensions
{
	internal class DapperQueryBuilder<T> : IQueryBuilder<T> where T : class
	{
		public IPredicate Predicate { get; private set; }

		public int? Limit { get; private set; }

		public int? Amount { get; private set; }

		public List<ISort> Sort { get; } = new List<ISort>();

		public List<IProjection> Projections { get; } = new List<IProjection>();

		public IQueryBuilder<T> Where(Action<IConditionBuilder<T>> action)
		{
			var condition = new DapperConditionBuilder<T>();

			action(condition);

			Predicate = condition.Predicate;

			return this;
		}

		public IQueryBuilder<T> Take(int limit)
		{
			Limit = limit;
			return this;
		}

		public IQueryBuilder<T> Skip(int amount)
		{
			Amount = amount;
			return this;
		}

		public IQueryBuilder<T> OrderBy(params Expression<Func<T, object>>[] fields)
		{
			foreach (var field in fields)
			{
				Sort.Add(Predicates.Sort(field));
			}

			return this;
		}

		public IQueryBuilder<T> OrderByDescending(params Expression<Func<T, object>>[] fields)
		{
			foreach (var field in fields)
			{
				Sort.Add(Predicates.Sort(field, false));
			}

			return this;
		}

		public IQueryBuilder<T> Select(params Expression<Func<T, object>>[] fields)
		{
			throw new NotImplementedException();
		}
	}
}