using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExistsForAll.DataStore.Core
{
	public interface IConditionBuilderTypes<T>
	{
		IConditionBuilderTypes<T> Equal<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilderTypes<T> NotEqual<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilderTypes<T> In<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value);

		IConditionBuilderTypes<T> NotIn<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value);

		IConditionBuilderTypes<T> GreaterOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilderTypes<T> GreaterThan<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilderTypes<T> LessOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilderTypes<T> LessThan<TValue>(Expression<Func<T, TValue>> member, TValue value);

		IConditionBuilderTypes<T> IsNull<TValue>(Expression<Func<T, TValue>> member);

		IConditionBuilderTypes<T> IsNotNull<TValue>(Expression<Func<T, TValue>> member);

		IConditionBuilderTypes<T> Or(Action<IConditionBuilderTypes<T>> orAction);
	}
}