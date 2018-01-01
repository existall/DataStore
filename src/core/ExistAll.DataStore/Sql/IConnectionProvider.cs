using System.Data;

namespace ExistAll.DataStore.Sql
{
	public interface IConnectionProvider
	{
		IDbConnection GetConnection();
	}
}