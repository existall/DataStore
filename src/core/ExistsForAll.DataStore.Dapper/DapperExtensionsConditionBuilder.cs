using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using DapperExtensions;
using ExistsForAll.DataStore.Core;

namespace ExistsForAll.DataStore.DapperExtensions
{
	internal class DapperExtensionsConditionBuilder<T> : IConditionBuilder<T> where T : class
	{
		public IPredicate Predicate { get; private set; }

		private void CombinePredicates(IPredicate predicate)
		{
			if (Predicate == null)
			{
				Predicate = predicate;
				return;
			}

			Predicate = Predicates.Group(GroupOperator.And, Predicate, predicate);
		}


		public IConditionBuilder<T> Equal(Expression<Func<T, object>> member, object value)
		{
			var predicate = Predicates.Field(member, Operator.Eq, value);
			CombinePredicates(predicate);
			return this;
		}

		public IConditionBuilder<T> NotEqual(Expression<Func<T, object>> member, object value)
		{
			var predicate = Predicates.Field(member, Operator.Eq, value, true);
			CombinePredicates(predicate);
			return this;
		}

		public IConditionBuilder<T> In(Expression<Func<T, object>> member, ICollection value)
		{
			var memberInfo = ReflectionHelper.GetProperty(member) as PropertyInfo;

			var inPredicate = new InPredicate<T>(value, memberInfo.Name);

			//var predicateList = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };

			//foreach (var item in value)
			//{
			//	Predicates.Field(member, Operator.Eq, item);
			//}

			CombinePredicates(inPredicate);

			return this;
		}

		public IConditionBuilder<T> NotIn(Expression<Func<T, object>> member, ICollection value)
		{
			var memberInfo = ReflectionHelper.GetProperty(member) as PropertyInfo;

			var inPredicate = new InPredicate<T>(value, memberInfo.Name, true);

			//var predicateList = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };

			//foreach (var item in value)
			//{
			//	Predicates.Field(member, Operator.Eq, item, true);
			//}

			CombinePredicates(inPredicate);

			return this;
		}

		public IConditionBuilder<T> GreaterOrEqual(Expression<Func<T, object>> member, object value)
		{
			return InnerFieldSet(member, Operator.Ge, value);
		}

		public IConditionBuilder<T> GreaterThan(Expression<Func<T, object>> member, object value)
		{
			return InnerFieldSet(member, Operator.Gt, value);
		}

		public IConditionBuilder<T> LessOrEqual(Expression<Func<T, object>> member, object value)
		{
			return InnerFieldSet(member, Operator.Le, value);
		}

		public IConditionBuilder<T> LessThan(Expression<Func<T, object>> member, object value)
		{
			return InnerFieldSet(member, Operator.Lt, value);
		}

		public IConditionBuilder<T> IsNull(Expression<Func<T, object>> member)
		{
			return InnerFieldSet(member, Operator.Eq, null);
		}

		public IConditionBuilder<T> IsNotNull(Expression<Func<T, object>> member)
		{
			return InnerFieldSet(member, Operator.Eq, null, true);
		}

		public IConditionBuilder<T> Or(Action<IConditionBuilder<T>> orAction)
		{
			var condition = new DapperExtensionsConditionBuilder<T>();

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

		private IConditionBuilder<T> InnerFieldSet(Expression<Func<T, object>> member, Operator @operator, object value, bool not = false)
		{
			var fieldPredicate = Predicates.Field(member, @operator, value, not);

			CombinePredicates(fieldPredicate);

			return this;
		}
	}
}