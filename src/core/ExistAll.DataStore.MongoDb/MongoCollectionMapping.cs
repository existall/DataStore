using System;

namespace ExistAll.DataStore.MongoDb
{
	public class MongoCollectionMapping
	{
		public MongoCollectionMapping(string schema,
			string collectionName,
			Type documentType,
			Type idType)
		{
			Schema = schema;
			CollectionName = collectionName;
			DocumentType = documentType;
			IdType = idType;
		}

		public string Schema { get; }
		public string CollectionName { get; }
		public Type DocumentType { get; }
		public Type IdType { get; }
	}
}