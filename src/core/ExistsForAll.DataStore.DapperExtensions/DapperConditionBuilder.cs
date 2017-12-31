using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExistAll.DataStore;
using ExistsForAll.DapperExtensions;
using ExistsForAll.DapperExtensions.Predicates;

namespace ExistsForAll.DataStore.DapperExtensions
{
	internal class DapperConditionBuilder<T> : IConditionBuilder<T> where T : class
	{
		public IPredicate Predicate { get; private set; }

		private void CombinePredicate(IPredicate predicate)
		{
			if (Predicate == null)
			{
				Predicate = predicate;
				return;
			}

			Predicates.Group(GroupOperator.And, Predicate, predicate);
		}

		public IConditionBuilder<T> Equal(Expression<Func<T, object>> member, object value)
		{
			return PredicateAction(() => Predicates.Field(member, Operator.Eq, value));
		}

		public IConditionBuilder<T> NotEqual(Expression<Func<T, object>> member, object value)
		{
			return PredicateAction(() => Predicates.Field(member, Operator.Eq, value, true));
		}

		public IConditionBuilder<T> In(Expression<Func<T, object>> member, ICollection<object> value)
		{
			return PredicateAction(() => Predicates.In(member, (ICollection)value));
		}

		public IConditionBuilder<T> NotIn(Expression<Func<T, object>> member, ICollection<object> value)
		{
			return PredicateAction(() => Predicates.In(member, (ICollection)value, true));
		}

		public IConditionBuilder<T> GreaterOrEqual(Expression<Func<T, object>> member, object value)
		{
			return PredicateAction(() => Predicates.Field(member, Operator.Ge, value));
		}

		public IConditionBuilder<T> GreaterThan(Expression<Func<T, object>> member, object value)
		{
			return PredicateAction(() => Predicates.Field(member, Operator.Gt, value));
		}

		public IConditionBuilder<T> LessOrEqual(Expression<Func<T, object>> member, object value)
		{
			return PredicateAction(() => Predicates.Field(member, Operator.Le, value));
		}

		public IConditionBuilder<T> LessThan(Expression<Func<T, object>> member, object value)
		{
			return PredicateAction(() => Predicates.Field(member, Operator.Lt, value));
		}

		public IConditionBuilder<T> IsNull(Expression<Func<T, object>> member)
		{
			return PredicateAction(() => Predicates.Field(member, Operator.Eq, null));
		}

		public IConditionBuilder<T> IsNotNull(Expression<Func<T, object>> member)
		{
			return PredicateAction(() => Predicates.Field(member, Operator.Eq, null, true));
		}

		public IConditionBuilder<T> Or(Action<IConditionBuilder<T>> orAction)
		{
			var condition = new DapperConditionBuilder<T>();

			orAction(condition);

			var predicate = condition.Predicate;

			if (predicate == null)
				return this;

			if (Predicate == null)
			{
				Predicate = predicate;
				return this;
			}

			Predicate = Predicates.Group(GroupOperator.Or, Predicate, predicate);
			return this;
		}

		private IConditionBuilder<T> PredicateAction(Func<IPredicate> func)
		{
			var predicate = func();
			CombinePredicate(predicate);
			return this;
		}
	}
}