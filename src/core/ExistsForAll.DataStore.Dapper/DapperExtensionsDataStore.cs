using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
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
				Predicates.
				con.

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

		public async Task<List<T>> GetByFieldAsync(Expression<Func<T, object>> selector, object key)
		{
			using (var con = _connectionProvider.GetOpenConnection())
			{
				var predicate = Predicates.Field(selector, Operator.Eq, key);

				var result = await con.GetListAsync<T>(predicate);
				return result.ToList();
			}
		}

		public async Task<List<T>> GetByFieldAsync(Expression<Func<T, object>> selector, IEnumerable<object> ids)
		{
			var query = new DapperExtensionsQueryBuilder<T>();

			query.Where(x => x.In(selector, ids.ToArray()));

			query.Predicate;

			using (var con = _connectionProvider.GetOpenConnection())
			{
				var result = await con.GetListAsync<T>();

				return result.ToList();
			}
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
			throw new NotImplementedException();

			// should be upsert not supported not by dapper - will be done.
		}

		public async Task UpdateAsync(T t)
		{
			using (var con = _connectionProvider.GetOpenConnection())
			{
				await con.UpdateAsync(t);
			}
		}
	}


	//public static class X
	//{

	//	public static void Upsert<T>(this IDapperImplementor implementor, IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class
	//	{
	//		IClassMapper classMap = implementor.SqlGenerator.Configuration.GetMap<T>();
	//		var properties = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);

	//		foreach (var column in properties)
	//		{
	//			if (column.KeyType == KeyType.Guid && (Guid)column.PropertyInfo.GetValue(entity, null) == Guid.Empty)
	//			{
	//				Guid comb = SqlGenerator.Configuration.GetNextGuid();
	//				column.PropertyInfo.SetValue(entity, comb, null);
	//			}
	//		}


	//		string sql = SqlGenerator.Insert(classMap);

	//		connection.Execute(sql, entity, transaction, commandTimeout, CommandType.Text);
	//	}

	//	public static string X(IClassMapper classMap, ISqlGenerator sqlGenerator)
	//	{
	//		int[] i = { 0 };

	//		var properties = classMap.Properties;

	//		var ids = properties
	//			.Where(x => x.KeyType == KeyType.Identity && !x.IsReadOnly)
	//			.Select(x => sqlGenerator.GetColumnName(classMap,x,true))
	//			.ToArray();

	//		var update = properties
	//			.Where(x => !x.IsReadOnly && !x.Ignored && x.KeyType != KeyType.Identity)
	//			.Select(x => new { name = sqlGenerator.GetColumnName(classMap, x, false), param = sqlGenerator.Configuration.Dialect.ParameterPrefix + x.Name})
	//			.ToArray();

	//		if (update.Length == 0)
	//			return string.Format("INSERT IGNORE INTO {0}({1}) VALUES ({2});",
	//				classMap.TableName,
	//				string.Join(", ", GetColumns().Select(x => x.ColumnName)),
	//				string.Join(", ", ids.Concat(update.Select(x => x.param)))
	//			);

	//		return string.Format("INSERT INTO {0}({1}) VALUES ({2}) ON DUPLICATE KEY UPDATE {3};",
	//			_map.TableName,
	//			string.Join(", ", GetColumns().Select(x => x.ColumnName)),
	//			string.Join(", ", ids.Concat(update.Select(x => x.param))),
	//			string.Join(", ", update.Select(x => x.name + " = " + x.param))
	//		);
	//	}
	//}
}
