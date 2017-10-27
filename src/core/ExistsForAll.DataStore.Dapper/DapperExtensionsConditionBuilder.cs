using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DapperExtensions;
using ExistsForAll.DataStore.Core;

namespace ExistsForAll.DataStore.DapperExtensions
{

	internal class Person
	{
		public string Name { get; set; }
		public int Age { get; set; }
	}

	internal class TestDataStore : DapperExtensionsDataStore<Person>
	{
		private readonly IDbConnectionProvider _connectionProvider;

		public TestDataStore(IDbConnectionProvider connectionProvider) 
			: base(connectionProvider)
		{
			_connectionProvider = connectionProvider;


			this.QueryAsync(x => x.Where(y => y.Equal(z => z.Age, 2)));

		}
	}

	public interface IX<T,TValue> :IConditionBuilder<T>
	{ }

	public class X<T> : IX<T,object>
	{
		public IConditionBuilder<T> Equal<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> NotEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> In<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> NotIn<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> GreaterOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> GreaterThan<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> LessOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> LessThan<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> IsNull<TValue>(Expression<Func<T, TValue>> member)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> IsNotNull<TValue>(Expression<Func<T, TValue>> member)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> Or(Action<IConditionBuilder<T>> orAction)
		{
			throw new NotImplementedException();
		}
	}



	internal class DapperExtensionsConditionBuilder<T> : IConditionBuilder<T> where T : class
	{
		PredicateGroup Group = new PredicateGroup();

		public IConditionBuilder<T> Equal<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			Expression<Func<T, object>> x = arg => arg;
			Predicates.Field(member.Body, Operator.Eq, value, true);
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> NotEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> In<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> NotIn<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> GreaterOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> GreaterThan<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> LessOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> LessThan<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> IsNull<TValue>(Expression<Func<T, TValue>> member)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> IsNotNull<TValue>(Expression<Func<T, TValue>> member)
		{
			throw new NotImplementedException();
		}

		public IConditionBuilder<T> Or(Action<IConditionBuilder<T>> orAction)
		{
			throw new NotImplementedException();
		}
	}
}