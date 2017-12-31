using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Impl;

namespace ExistAll.DataStore.NHibernate
{
	public class NHibernateDataStore<T> : ITypeDataStore<T> where T : class
	{
		private readonly ISessionFactory _factoryProvider;

		public NHibernateDataStore(ISessionFactory factoryProvider)
		{
			_factoryProvider = factoryProvider;
		}

		public Task<List<T>> QueryAsync(Action<ITypeQueryBuilder<T>> queryManipulator)
		{
			using (var session = _factoryProvider.OpenStatelessSession())
			{
				var query = new NHibernateQueryBuilder<T>();
				queryManipulator(query);

				var results = query
					.Criteria
					.GetExecutableCriteria(session)
					.List<T>();

				return Task.FromResult((List<T>)results);
			}
		}

		public Task<List<T>> QueryAllAsync()
		{
			using (var session = _factoryProvider.OpenStatelessSession())
			{
				var results = session
					.CreateCriteria<T>()
					.List<T>();

				return Task.FromResult((List<T>)results);
			}
		}

		public Task<long> CountAsync(Action<ITypeQueryBuilder<T>> queryManipulator = null)
		{
			using (var session = _factoryProvider.OpenStatelessSession())
			{
				var query = new NHibernateQueryBuilder<T>();

				queryManipulator?.Invoke(query);

				var count = query
					.Criteria
					.GetExecutableCriteria(session)
					.SetProjection(Projections.RowCount())
					.UniqueResult<long>();

				return Task.FromResult(count);
			}
		}

		public Task<T> GetByIdAsync(T id)
		{
			using (var session = _factoryProvider.OpenStatelessSession())
			{
				return Task.FromResult(session.Get<T>(id));
			}
		}

		public Task AddAsync(T t)
		{
			using (var session = _factoryProvider.OpenStatelessSession())
			{
				session.Insert(t);
			}

			return Task.FromResult(0);
		}

		public Task SaveAsync(T t)
		{
			using (var session = _factoryProvider.OpenSession())
			{
				session.SaveOrUpdate(t);
				session.Flush();
			}

			return Task.FromResult(0);
		}

		public Task UpdateAsync(T t)
		{
			using (var session = _factoryProvider.OpenStatelessSession())
			{
				session.Update(t);
			}

			return Task.FromResult(0);
		}

		public Task DeleteAsync(T t)
		{
			using (var session = _factoryProvider.OpenSession())
			{
				session.Delete(t);
				session.Flush();
			}

			return Task.FromResult(0);
		}

		public Task<List<T>> GetByFieldAsync<TKey>(Expression<Func<T, TKey>> selector, TKey key)
		{
			var field = ExpressionProcessor.FindMemberExpression(selector.Body);
			using (var session = _factoryProvider.OpenStatelessSession())
			{
				var dc = DetachedCriteria.For<T>().Add(Restrictions.Eq(field, key));
				var result = dc
					.GetExecutableCriteria(session)
					.List<T>()
					.ToList();

				return Task.FromResult(result);
			}
		}

		public Task<List<T>> GetByFieldAsync<TKey>(Expression<Func<T, TKey>> selector, IEnumerable<TKey> ids)
		{
			var field = ExpressionProcessor.FindMemberExpression(selector.Body);
			using (var session = _factoryProvider.OpenStatelessSession())
			{
				var dc = DetachedCriteria.For<T>().Add(Restrictions.InG(field, ids));
				var result = dc
					.GetExecutableCriteria(session)
					.List<T>()
					.ToList();

				return Task.FromResult(result);
			}
		}
	}
}