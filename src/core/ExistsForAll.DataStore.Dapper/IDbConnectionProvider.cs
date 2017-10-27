using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ExistsForAll.DataStore.DapperExtensions
{
	public interface IDbConnectionProvider
	{
		IDbConnection GetConnection();
	}

	public class PostgreSqlConnectionProvider : IDbConnectionProvider
	{
		public IDbConnection GetConnection()
		{
			return null;
		}

		public void UseConnection(Action<IDbConnection> action)
		{
			using (var connection = GetConnection())
			{
				action.Invoke(connection);
			}
		}

	}
}