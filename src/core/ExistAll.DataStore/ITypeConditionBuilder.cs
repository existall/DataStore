using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExistAll.DataStore
{
	public interface ITypeConditionBuilder<T>
	{
		ITypeConditionBuilder<T> Equal<TValue>(Expression<Func<T, TValue>> member, TValue value);

		ITypeConditionBuilder<T> NotEqual<TValue>(Expression<Func<T, TValue>> member, TValue value);

		ITypeConditionBuilder<T> In<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value);

		ITypeConditionBuilder<T> NotIn<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value);

		ITypeConditionBuilder<T> GreaterOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value);

		ITypeConditionBuilder<T> GreaterThan<TValue>(Expression<Func<T, TValue>> member, TValue value);

		ITypeConditionBuilder<T> LessOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value);

		ITypeConditionBuilder<T> LessThan<TValue>(Expression<Func<T, TValue>> member, TValue value);

		ITypeConditionBuilder<T> IsNull<TValue>(Expression<Func<T, TValue>> member);

		ITypeConditionBuilder<T> IsNotNull<TValue>(Expression<Func<T, TValue>> member);

		ITypeConditionBuilder<T> Or(Action<ITypeConditionBuilder<T>> orAction);
	}
}