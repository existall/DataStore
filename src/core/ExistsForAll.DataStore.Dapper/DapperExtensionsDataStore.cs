using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
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
			using (var con = _connectionProvider.GetOpenConnection())
			{
				await con.InsertAsync(t);
			}
		}

		public async Task<long> CountAsync(Action<IQueryBuilder<T>> queryManipulator = null)
		{
			long count = 0;

			await _connectionProvider.UseOpenConnectionAsync(async c =>
			{
				count = await c.CountAsync<T>(c);
			});

			return await Task.FromResult(count);
		}

		public async Task DeleteAsync(T t)
		{
			await _connectionProvider.UseOpenConnectionAsync(async c =>
			{
				await c.DeleteAsync(t);
			});
		}

		public Task<List<T>> GetByFieldAsync<TKey>(Expression<Func<T, TKey>> selector, TKey key)
		{
			throw new NotImplementedException();
		}

		public Task<List<T>> GetByFieldAsync<TKey>(Expression<Func<T, TKey>> selector, IEnumerable<TKey> ids)
		{
			throw new NotImplementedException();
		}

		public async Task<T> GetByIdAsync(object id)
		{
			using (var con = _connectionProvider.GetOpenConnection())
			{
				return await con.GetAsync<T>(id);
			}
		}

		public async Task<List<T>> QueryAllAsync()
		{
			using (var con = _connectionProvider.GetOpenConnection())
			{
				var result = await con.GetListAsync<T>();
				return result.ToList();
			}
		}



		public Task<List<T>> QueryAsync(Action<IQueryBuilder<T>> queryManipulator)
		{
			throw new NotImplementedException();
		}

		public async Task SaveAsync(T t)
		{
			using (var con = _connectionProvider.GetOpenConnection())
			{
				await con.(t);
			}
		}

		public async Task UpdateAsync(T t)
		{
			using (var con = _connectionProvider.GetOpenConnection())
			{
				await con.UpdateAsync(t);
			}
		}
	}


	public static class X
	{

		public static void Upsert<T>(this IDapperImplementor implementor, IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class
		{
			IClassMapper classMap = implementor.SqlGenerator.Configuration.GetMap<T>();
			var properties = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);

			foreach (var column in properties)
			{
				if (column.KeyType == KeyType.Guid && (Guid)column.PropertyInfo.GetValue(entity, null) == Guid.Empty)
				{
					Guid comb = SqlGenerator.Configuration.GetNextGuid();
					column.PropertyInfo.SetValue(entity, comb, null);
				}
			}


			string sql = SqlGenerator.Insert(classMap);

			connection.Execute(sql, entity, transaction, commandTimeout, CommandType.Text);
		}

		public static string X(IClassMapper classMap)
		{
			int[] i = { 0 };

			var ids = classMap.Properties
				.Where(x => x.KeyType == KeyType.Identity && !x.IsReadOnly)
				.Select(x => _map.CreateParameter("p" + i[0]++))
				.ToArray();

			var update = _map
				.Properties
				.Where(x => !x.ReadOnly)
				.Select(x => new { name = x.ColumnName, param = _map.CreateParameter("p" + i[0]++) })
				.ToArray();

			if (update.Length == 0)
				return string.Format("INSERT IGNORE INTO {0}({1}) VALUES ({2});",
					_map.TableName,
					string.Join(", ", GetColumns().Select(x => x.ColumnName)),
					string.Join(", ", ids.Concat(update.Select(x => x.param)))
				);

			return string.Format("INSERT INTO {0}({1}) VALUES ({2}) ON DUPLICATE KEY UPDATE {3};",
				_map.TableName,
				string.Join(", ", GetColumns().Select(x => x.ColumnName)),
				string.Join(", ", ids.Concat(update.Select(x => x.param))),
				string.Join(", ", update.Select(x => x.name + " = " + x.param))
			);
		}
	}
}
