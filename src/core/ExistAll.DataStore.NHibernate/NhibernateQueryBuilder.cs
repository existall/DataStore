using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Impl;
using NHibernate.Transform;

namespace ExistAll.DataStore.NHibernate
{
	public class NHibernateQueryBuilder<T> : ITypeQueryBuilder<T>
	{
		public DetachedCriteria Criteria { get; private set; }

		public NHibernateQueryBuilder()
		{
			Criteria = DetachedCriteria.For<T>();
		}

		private string GetProperty<TValue>(Expression<Func<T, TValue>> member)
		{
			return ExpressionProcessor.FindMemberExpression(member.Body);
		}

		public ITypeQueryBuilder<T> Where(Action<ITypeConditionBuilder<T>> action)
		{
			var condition = new NHibernateConditionBuilder<T>();

			action(condition);

			var criterion = condition.Criterion;

			if (criterion == null)
				return this;

			Criteria.Add(criterion);

			return this;
		}

		public ITypeQueryBuilder<T> Take(int limit)
		{
			Criteria.SetMaxResults(limit);
			return this;
		}

		public ITypeQueryBuilder<T> Skip(int amount)
		{
			Criteria.SetFirstResult(amount);
			return this;
		}

		public ITypeQueryBuilder<T> OrderBy(params Expression<Func<T, object>>[] fields)
		{
			foreach (var expression in fields)
				Criteria
					.AddOrder(Order.Asc(GetProperty(expression)));

			return this;
		}

		public ITypeQueryBuilder<T> OrderByDescending(params Expression<Func<T, object>>[] fields)
		{
			foreach (var expression in fields)
				Criteria
					.AddOrder(Order.Desc(GetProperty(expression)));

			return this;
		}

		public ITypeQueryBuilder<T> DistinctBy(params Expression<Func<T, object>>[] fields)
		{
			var projectionList = Projections.ProjectionList();

			foreach (var expression in fields)
				projectionList
					.Add(Projections.Distinct(Projections.Property(expression)),
						ExpressionProcessor.FindMemberExpression(expression.Body));

			Criteria
				.SetProjection(projectionList)
				.SetResultTransformer(Transformers.AliasToBean<T>());

			return this;
		}

		public ITypeQueryBuilder<T> Select(params Expression<Func<T, object>>[] fields)
		{
			var projectionList = Projections.ProjectionList();

			foreach (var expression in fields)
				projectionList
					.Add(Projections.Property(expression), ExpressionProcessor.FindMemberExpression(expression.Body));

			Criteria
				.SetProjection(projectionList)
				.SetResultTransformer(Transformers.AliasToBean<T>());

			return this;
		}

		public ITypeQueryBuilder<T> StartWith(Expression<Func<T, string>> member, string value)
		{
			Criteria.Add(Restrictions.Like(GetProperty(member), value, MatchMode.Start));
			return this;
		}

		public ITypeQueryBuilder<T> Contains(Expression<Func<T, string>> member, string value)
		{
			Criteria.Add(Restrictions.Like(GetProperty(member), value, MatchMode.Anywhere));
			return this;
		}
	}
}