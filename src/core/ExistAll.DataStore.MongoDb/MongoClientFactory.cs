using System;
using System.Collections.Generic;
using ExistAll.DataStore.Collections;
using MongoDB.Driver;

namespace ExistAll.DataStore.MongoDb
{
	public class MongoClientFactory : IMongoClientFactory
	{
		private readonly IDictionary<string, string> _connectionStringsBySchema;
		private readonly IDictionary<Type, MongoCollectionMapping> _collectionMappingByType;

		public IEnumerable<string> Schemas => _connectionStringsBySchema.Keys;

		public MongoClientFactory(IDictionary<string, string> connectionStringsBySchema,
			IDictionary<Type, MongoCollectionMapping> collectionMappingByType)
		{
			_connectionStringsBySchema = connectionStringsBySchema;
			_collectionMappingByType = collectionMappingByType;
		}

		public IMongoCollection<T> GetCollection<T>()
		{
			var collectionMap = FindCollectionMap<T>();
			var database = GetDatabase(collectionMap.Schema);
			return database.GetCollection<T>(collectionMap.CollectionName);
		}

		private MongoCollectionMapping FindCollectionMap<T>()
		{
			var collectionMap = _collectionMappingByType.ItemOrDefault(typeof(T));
			if (collectionMap == null)
				throw new InvalidOperationException("No Mongo collection map found for type: " + typeof(T));
			return collectionMap;
		}

		public IMongoDatabase GetDatabase(string schema)
		{
			var connectionString = _connectionStringsBySchema.ItemOrDefault(schema);
			if (connectionString == null)
				throw new InvalidOperationException("No connection string found for Mongo schema: " + schema);

			var mongoUrl = MongoUrl.Create(connectionString);
			var mongoClient = new MongoClient(mongoUrl);

			var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
			return database;
		}
	}
}