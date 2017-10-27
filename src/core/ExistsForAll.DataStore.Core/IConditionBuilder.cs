using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExistsForAll.DataStore.Core
{
	public interface IConditionBuilder<T>
	{
		IConditionBuilder<T> Equal<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilder<T> NotEqual<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilder<T> In<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value);

		IConditionBuilder<T> NotIn<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value);

		IConditionBuilder<T> GreaterOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilder<T> GreaterThan<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilder<T> LessOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilder<T> LessThan<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilder<T> IsNull<TValue>(Expression<Func<T, TValue>> member);

		IConditionBuilder<T> IsNotNull<TValue>(Expression<Func<T, TValue>> member);

		IConditionBuilder<T> Or(Action<IConditionBuilder<T>> orAction);
	}
}