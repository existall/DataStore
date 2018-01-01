using System;
using System.Data;
using System.Threading.Tasks;
using ExistAll.DataStore.Sql;

namespace ExistsForAll.DataStore.DapperExtensions
{
	internal static class ConnectionProviderExtensions
	{
		public static IDbConnection GetOpenConnection(this IConnectionProvider target)
		{
			var dbConnection = target.GetConnection();
			if (dbConnection.State == ConnectionState.Open)
				return dbConnection;

			dbConnection.Open();
			return dbConnection;
		}

		public static void UseConnection(this IConnectionProvider target, Action<IDbConnection> action)
		{
			using (var connection = target.GetOpenConnection())
			{
				action(connection);
			}
		}

		public static async Task UseConnectionAsync(this IConnectionProvider target, Func<IDbConnection,Task> action)
		{
			using (var connection = target.GetOpenConnection())
			{
				await action(connection);
			}
		}

		public static async Task<T> UseConnectionResultAsync<T>(this IConnectionProvider target, Func<IDbConnection, Task<T>> action)
		{
			using (var connection = target.GetOpenConnection())
			{
				return await action(connection);
			}
		}

		public static T UseConnection<T>(this IConnectionProvider target, Func<IDbConnection, T> func)
		{
			using (var connection = target.GetOpenConnection())
			{
				return func(connection);
			}
		}

		public static async Task<T> UseConnection<T>(this IConnectionProvider target, Func<IDbConnection, Task<T>> func)
		{
			using (var connection = target.GetOpenConnection())
			{
				return await func(connection);
			}
		}
	}
}