﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExistAll.DataStore
{
	public interface IConditionBuilder<T>
	{
		IConditionBuilder<T> Equal(Expression<Func<T, object>> member, object value);
						 
		IConditionBuilder<T> NotEqual(Expression<Func<T, object>> member, object value);
						 
		IConditionBuilder<T> In(Expression<Func<T, object>> member, ICollection<object> value);
						 
		IConditionBuilder<T> NotIn(Expression<Func<T, object>> member, ICollection<object> value);
						 
		IConditionBuilder<T> GreaterOrEqual(Expression<Func<T, object>> member, object value);
						 
		IConditionBuilder<T> GreaterThan(Expression<Func<T, object>> member, object value);
						 
		IConditionBuilder<T> LessOrEqual(Expression<Func<T, object>> member, object value);
						 
		IConditionBuilder<T> LessThan(Expression<Func<T, object>> member, object value);
						 
		IConditionBuilder<T> IsNull(Expression<Func<T, object>> member);
						 
		IConditionBuilder<T> IsNotNull(Expression<Func<T, object>> member);
						 
		IConditionBuilder<T> Or(Action<IConditionBuilder<T>> orAction);
	}
}