using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace ExistAll.DataStore.MongoDb
{
	internal abstract class MongoDbConditionBuilder<T> : ITypeConditionBuilder<T>
	{
		protected readonly FilterDefinitionBuilder<T> Builder = Builders<T>.Filter;

		protected abstract FilterDefinition<T> CombineFilter(FilterDefinition<T> aggregatedFilter, FilterDefinition<T> filter);

		public FilterDefinition<T> Filters { get; private set; }

		private void CombineFilters(Func<FilterDefinitionBuilder<T>, FilterDefinition<T>> filterBuilder)
		{
			Filters = Filters == null ? filterBuilder(Builder) : CombineFilter(Filters, filterBuilder(Builder));
		}

		public ITypeConditionBuilder<T> Equal<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineFilters(x => x.Eq(member, value));
			return this;
		}

		public ITypeConditionBuilder<T> NotEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineFilters(x => x.Ne(member, value));
			return this;
		}

		public ITypeConditionBuilder<T> In<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value)
		{
			CombineFilters(x => x.In(member, value));
			return this;
		}

		public ITypeConditionBuilder<T> NotIn<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value)
		{
			CombineFilters(x => x.Nin(member, value));
			return this;
		}

		public ITypeConditionBuilder<T> GreaterOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineFilters(x => x.Gte(member, value));
			return this;
		}

		public ITypeConditionBuilder<T> GreaterThan<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineFilters(x => x.Gt(member, value));
			return this;
		}

		public ITypeConditionBuilder<T> LessOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineFilters(x => x.Lte(member, value));
			return this;
		}

		public ITypeConditionBuilder<T> LessThan<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineFilters(x => x.Lt(member, value));
			return this;
		}

		public ITypeConditionBuilder<T> IsNull<TValue>(Expression<Func<T, TValue>> member)
		{
			ThrowIfNotNullable<TValue>();
			CombineFilters(x => x.Eq(member, default(TValue)));
			return this;
		}

		public ITypeConditionBuilder<T> IsNotNull<TValue>(Expression<Func<T, TValue>> member)
		{
			ThrowIfNotNullable<TValue>();
			CombineFilters(x => x.Ne(member, default(TValue)));
			return this;
		}

		public ITypeConditionBuilder<T> Or(Action<ITypeConditionBuilder<T>> orAction)
		{
			var conditionBuilder = new DisjunctionMongoDbConditionBuilder<T>();

			orAction(conditionBuilder);

			CombineFilters(_ => conditionBuilder.Filters);
			return this;
		}

		private static void ThrowIfNotNullable<TValue>()
		{
			if (ReferenceEquals(default(TValue), null))
				return;

			throw new ArgumentException("Member cannot be equated with null as it is not Nullable<T> or class (such as string for instance)");
		}
	}
}