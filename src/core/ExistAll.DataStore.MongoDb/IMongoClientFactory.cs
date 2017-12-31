using System.Collections.Generic;
using MongoDB.Driver;

namespace ExistAll.DataStore.MongoDb
{
	public interface IMongoClientFactory
	{
		IMongoCollection<T> GetCollection<T>();
		IMongoDatabase GetDatabase(string schema);

		IEnumerable<string> Schemas { get; }
	}
}