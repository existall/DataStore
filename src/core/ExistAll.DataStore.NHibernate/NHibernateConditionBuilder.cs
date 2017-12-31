using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Impl;

namespace ExistAll.DataStore.NHibernate
{
	public class NHibernateConditionBuilder<T> : ITypeConditionBuilder<T>
	{
		public ICriterion Criterion { get;  private set; }

		private void CombineRestriction(ICriterion restriction)
		{
			if (Criterion == null)
				Criterion = restriction;

			Criterion = Restrictions.And(Criterion, restriction);
		} 
		

		private string GetProperty<TValue>(Expression<Func<T, TValue>> member)
		{
			return ExpressionProcessor.FindMemberExpression(member.Body);
		}

		public ITypeConditionBuilder<T> Equal<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineRestriction(Restrictions.Eq(GetProperty(member), value));
			return this;
		}

		public ITypeConditionBuilder<T> NotEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineRestriction(Restrictions.Not(Restrictions.Eq(GetProperty(member), value)));
			return this;
		}

		public ITypeConditionBuilder<T> In<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value)
		{
			CombineRestriction(Restrictions.InG(GetProperty(member), value));
			return this;
		}

		public ITypeConditionBuilder<T> NotIn<TValue>(Expression<Func<T, TValue>> member, ICollection<TValue> value)
		{
			CombineRestriction(Restrictions.Not(Restrictions.InG(GetProperty(member), value)));
			return this;
		}

		public ITypeConditionBuilder<T> GreaterOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineRestriction(Restrictions.Ge(GetProperty(member), value));
			return this;
		}

		public ITypeConditionBuilder<T> GreaterThan<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineRestriction(Restrictions.Gt(GetProperty(member), value));
			return this;
		}

		public ITypeConditionBuilder<T> LessOrEqual<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineRestriction(Restrictions.Le(GetProperty(member), value));
			return this;
		}

		public ITypeConditionBuilder<T> LessThan<TValue>(Expression<Func<T, TValue>> member, TValue value)
		{
			CombineRestriction(Restrictions.Lt(GetProperty(member), value));
			return this;
		}

		public ITypeConditionBuilder<T> IsNull<TValue>(Expression<Func<T, TValue>> member)
		{
			CombineRestriction(Restrictions.IsNull(GetProperty(member)));
			return this;
		}

		public ITypeConditionBuilder<T> IsNotNull<TValue>(Expression<Func<T, TValue>> member)
		{
			CombineRestriction(Restrictions.IsNotNull(GetProperty(member)));
			return this;
		}

		public ITypeConditionBuilder<T> Or(Action<ITypeConditionBuilder<T>> orAction)
		{
			var condition = new NHibernateConditionBuilder<T>();

			orAction(condition);

			var criterion = condition.Criterion;

			if (criterion == null)
				return this;

			if (Criterion == null)
				Criterion = criterion;

			Criterion = Restrictions.Or(Criterion, condition.Criterion);
			return this;
		}
	}
}