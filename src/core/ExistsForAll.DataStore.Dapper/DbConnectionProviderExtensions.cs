using System;
using System.Data;
using System.Threading.Tasks;

namespace ExistsForAll.DataStore.DapperExtensions
{
	public static class DbConnectionProviderExtensions
	{
		public static IDbConnection GetOpenConnection(this IDbConnectionProvider target)
		{
			var connection = target.GetConnection();
			connection.Open();
			return connection;
		}

		public static async Task UseOpenConnectionAsync(this IDbConnectionProvider target, Func<IDbConnection, Task> func)
		{
			using (var connection = target.GetConnection())
			{
				connection.Open();
				await func(connection);
			}
		}

		public static void UseOpenConnection(this IDbConnectionProvider target, Action<IDbConnection> action)
		{
			using (var connection = target.GetConnection())
			{
				connection.Open();
				action(connection);
			}
		}
	}
}