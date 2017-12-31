using System;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace ExistAll.DataStore.MongoDb
{
	public class MongoDbQueryBuilder<T> : ITypeQueryBuilder<T>
	{
		private int _take;
		private int _skip;
		private FilterDefinition<T> _predicates;
		private SortDefinition<T> _sortby;
		private ProjectionDefinition<T> _fields;
		private FilterDefinitionBuilder<T> Filters => Builders<T>.Filter;

		public ProjectionDefinition<T> GetFields()
		{
			return _fields;
		}

		public int GetTake()
		{
			return _take;
		}

		public int GetSkip()
		{
			return _skip;
		}

		public FilterDefinition<T> GetFilters()
		{
			return _predicates;
		}

		public SortDefinition<T> GetSortOrder()
		{
			return _sortby;
		}

		public ITypeQueryBuilder<T> Where(Action<ITypeConditionBuilder<T>> action)
		{
			var conditionBuider = new ConjuctionMongoDbConditionBuilder<T>();

			action(conditionBuider);

			_predicates = conditionBuider.Filters;

			return this;
		}

		public ITypeQueryBuilder<T> Take(int limit)
		{
			_take = limit;
			return this;
		}

		public ITypeQueryBuilder<T> Skip(int amount)
		{
			_skip = amount;
			return this;
		}

		public ITypeQueryBuilder<T> OrderBy(params Expression<Func<T, object>>[] fields)
		{
			var builder = Builders<T>.Sort;

			foreach (var field in fields)
			{
				_sortby = builder.Combine(_sortby, builder.Ascending(field));
			}

			return this;
		}

		public ITypeQueryBuilder<T> OrderByDescending(params Expression<Func<T, object>>[] fields)
		{
			var builder = Builders<T>.Sort;

			foreach (var field in fields)
			{
				_sortby = builder.Combine(_sortby, builder.Descending(field));
			}

			return this;
		}

		public ITypeQueryBuilder<T> DistinctBy(params Expression<Func<T, object>>[] fields)
		{
			throw new NotImplementedException();
		}

		public ITypeQueryBuilder<T> Select(params Expression<Func<T, object>>[] fields)
		{
			var builder = Builders<T>.Projection;

			foreach (var field in fields)
			{
				_fields = builder.Combine(_fields, builder.Include(field));
			}

			return this;
		}
	}
}
