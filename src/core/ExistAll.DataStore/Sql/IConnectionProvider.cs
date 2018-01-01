using System.Data;

namespace ExistsForAll.DataStore.DapperExtensions
{
	public interface IConnectionProvider
	{
		IDbConnection GetConnection();
	}
}