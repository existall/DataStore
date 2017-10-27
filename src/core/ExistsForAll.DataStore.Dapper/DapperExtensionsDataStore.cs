using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DapperExtensions;
using ExistsForAll.DataStore.Core;

namespace ExistsForAll.DataStore.DapperExtensions
{
	public class DapperExtensionsDataStore<T> : IDataStore<T> where T : class
	{
		private readonly IDbConnectionProvider _connectionProvider;

		public DapperExtensionsDataStore(IDbConnectionProvider connectionProvider)
		{
			_connectionProvider = connectionProvider;
		}

		public async Task AddAsync(T t)
		{
			await _connectionProvider.UseOpenConnectionAsync(c => c.InsertAsync(t));
		}

		public async Task<long> CountAsync(Action<IQueryBuilder<T>> queryManipulator = null)
		{
			await _connectionProvider.UseOpenConnectionAsync(c => c.CountAsync<T>(c));
		}

		public Task DeleteAsync(T t)
		{
			throw new NotImplementedException();
		}

		public Task<List<T>> GetByFieldAsync<TKey>(System.Linq.Expressions.Expression<Func<T, TKey>> selector, TKey key)
		{
			throw new NotImplementedException();
		}

		public Task<List<T>> GetByFieldAsync<TKey>(System.Linq.Expressions.Expression<Func<T, TKey>> selector, IEnumerable<TKey> ids)
		{
			throw new NotImplementedException();
		}

		public Task<T> GetByIdAsync(T id)
		{
			throw new NotImplementedException();
		}

		public Task<List<T>> QueryAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<List<T>> QueryAsync(Action<IQueryBuilder<T>> queryManipulator)
		{
			throw new NotImplementedException();
		}

		public Task SaveAsync(T t)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(T t)
		{
			throw new NotImplementedException();
		}
	}
}
