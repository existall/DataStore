using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DapperExtensions;
using ExistsForAll.DataStore.Core;

namespace ExistsForAll.DataStore.DapperExtensions
{
	internal class DapperExtensionsQueryBuilder<T> : IQueryBuilder<T>
	{
		private readonly List<ISort> _sorts = new List<ISort>();

		public IPredicate Predicate { get; private set; }
		public int TakeLimit { get; private set; }
		public int SkipAmount { get; private set; }

		public IEnumerable<ISort> Sorts => _sorts;

		public IQueryBuilder<T> Where(Action<IConditionBuilder<T>> action)
		{
			//Predicates.

			//Predicate.
			throw new NotImplementedException();

		}

		public IQueryBuilder<T> Take(int limit)
		{
			if (limit < 0)
				throw new ArgumentException($"{nameof(limit)} value can't be less than zero.");

			TakeLimit = limit;
			return this;
		}

		public IQueryBuilder<T> Skip(int amount)
		{
			if (amount < 0)
				throw new ArgumentException($"{nameof(amount)} value can't be less than zero.");

			SkipAmount = amount;
			return this;
		}

		public IQueryBuilder<T> OrderBy(params Expression<Func<T, object>>[] fields)
		{
			foreach (var field in fields)
			{
				_sorts.Add(Predicates.Sort(field));
			}
			return this;
		}

		public IQueryBuilder<T> OrderByDescending(params Expression<Func<T, object>>[] fields)
		{
			foreach (var field in fields)
			{
				_sorts.Add(Predicates.Sort(field, false));
			}
			return this;
		}

		public IQueryBuilder<T> Select(params Expression<Func<T, object>>[] fields)
		{
			throw new NotImplementedException();
		}
	}
}