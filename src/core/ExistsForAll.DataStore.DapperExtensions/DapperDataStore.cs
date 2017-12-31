using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExistAll.DataStore;
using ExistsForAll.DapperExtensions;

namespace ExistsForAll.DataStore.DapperExtensions
{
	public class DapperDataStore<T> : IDataStore<T> where T : class
	{
		private readonly IDapperImplementor _dapper;
		private readonly IConnectionProvider _connectionProvider;

		public DapperDataStore(IDapperImplementor dapper, IConnectionProvider connectionProvider)
		{
			_dapper = dapper;
			_connectionProvider = connectionProvider;
		}

		public void Add(T t)
		{
			_connectionProvider.UseConnection(c => _dapper.Insert(c, t, null, null));
		}

		public long Count(Action<IQueryBuilder<T>> queryManipulator = null)
		{
			var query = new DapperQueryBuilder<T>();

			queryManipulator?.Invoke(query);

			return _connectionProvider.UseConnection(c => _dapper.Count<T>(c, query.Predicate, null, null));
		}

		public void Delete(T t)
		{
			_connectionProvider.UseConnection(x => _dapper.Delete(x, (T) t, null, null));
		}

		public T GetById(T id)
		{
			return _connectionProvider.UseConnection(c => _dapper.Get<T>(c, id, null, null));
		}

		public IEnumerable<T> QueryAll()
		{
			var result = _connectionProvider.UseConnection(c => _dapper.GetList<T>(c, null, null, null, null, false));

			return result.ToArray();
		}

		public IEnumerable<T> Query(Action<IQueryBuilder<T>> queryManipulator)
		{
			if (queryManipulator == null)
				throw new ArgumentNullException(nameof(queryManipulator));

			var query = new DapperQueryBuilder<T>();

			queryManipulator?.Invoke(query);

			IEnumerable<T> results = null;

			if (query.Limit.HasValue || query.Amount.HasValue)
			{
				results = _connectionProvider.UseConnection(c =>
					_dapper.GetSet<T>(c,
						query.Predicate,
						query.Sort,
						query.Amount ?? 1,
						query.Limit ?? 10,
						null,
						null,
						false,
						query.Projections));
			}
			else
			{
				results = _connectionProvider.UseConnection(c =>
					_dapper.GetList<T>(c,
						query.Predicate,
						query.Sort,
						null,
						null,
						false,
						query.Projections));
			}

			return results.ToArray();
		}

		public void Save(T t)
		{
			_connectionProvider.UseConnection(c => _dapper.Upsert(c, t, null, null));
		}

		public void Update(T t)
		{
			_connectionProvider.UseConnection(c => _dapper.Upsert(c, t, null, null));
		}
	}
}