using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExistAll.DataStore;
using ExistAll.DataStore.Sql;
using ExistsForAll.DapperExtensions;

namespace ExistsForAll.DataStore.DapperExtensions
{
	public class DapperDataStoreAsync<T> : IDataStoreAsync<T> where T : class
	{
		private readonly IDapperAsyncImplementor _dapper;
		private readonly IConnectionProvider _connectionProvider;

		public DapperDataStoreAsync(IDapperAsyncImplementor dapper, IConnectionProvider connectionProvider)
		{
			_dapper = dapper;
			_connectionProvider = connectionProvider;
		}

		public async Task AddAsync(T t)
		{
			await _connectionProvider.UseConnectionAsync(c => _dapper.InsertAsync(c, t));
		}

		public async Task<long> CountAsync(Action<IConditionBuilder<T>> conditionManipulator = null)
		{
			var condition = new DapperConditionBuilder<T>();

			conditionManipulator?.Invoke(condition);

			return await _connectionProvider.UseConnectionResultAsync(async c =>
				await _dapper.CountAsync<T>(c, condition.Predicate, null, null));
		}

		public async Task DeleteAsync(T t)
		{
			await _connectionProvider.UseConnectionAsync(async x => await _dapper.DeleteAsync(x, (T) t, null, null));
		}

		public async Task<T> GetByIdAsync(T id)
		{
			return await _connectionProvider.UseConnectionResultAsync(async c => await _dapper.GetAsync<T>(c, id));
		}

		public async Task<IEnumerable<T>> QueryAllAsync()
		{
			var result = await _connectionProvider.UseConnectionResultAsync(async c => await _dapper.GetListAsync<T>(c));

			return result.ToList();
		}

		public async Task<IEnumerable<T>> QueryAsync(Action<IQueryBuilder<T>> queryManipulator)
		{
			if (queryManipulator == null)
				throw new ArgumentNullException(nameof(queryManipulator));

			var query = new DapperQueryBuilder<T>();

			queryManipulator?.Invoke(query);

			IEnumerable<T> results = null;

			if (query.Limit.HasValue || query.Amount.HasValue)
			{
				results = await _connectionProvider.UseConnectionResultAsync(async c =>
					await _dapper.GetSetAsync<T>(c, query.Predicate, query.Sort, query.Amount ?? 1, query.Limit ?? 10,
						projections: query.Projections));
			}
			else
			{
				results = await _connectionProvider.UseConnectionResultAsync(async c =>
					await _dapper.GetListAsync<T>(c, query.Predicate, query.Sort, null, projections: query.Projections));
			}

			return results.ToList();
		}

		public async Task SaveAsync(T t)
		{
			await _connectionProvider.UseConnectionAsync(c => _dapper.Upsert(c, t, null, null));
		}

		public async Task UpdateAsync(T t)
		{
			await _connectionProvider.UseConnectionAsync(c => _dapper.UpdateAsync(c, t, null, null));
		}
	}
}