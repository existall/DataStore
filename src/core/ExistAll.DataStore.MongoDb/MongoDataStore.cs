using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;

namespace ExistAll.DataStore.MongoDb
{
	public class MongoDataStore<T, TId> : ITypeDataStore<T> where T : class, IEntity<TId>
	{
		private readonly IMongoClientFactory _mongoClientFactory;

		public MongoDataStore(IMongoClientFactory mongoClientFactory)
		{
			_mongoClientFactory = mongoClientFactory;
		}

		private IMongoCollection<T> GetCollection()
		{
			return _mongoClientFactory.GetCollection<T>();
		}

		public Task<List<T>> QueryAsync(Action<ITypeQueryBuilder<T>> queryManipulator)
		{
			var query = new MongoDbQueryBuilder<T>();

			queryManipulator(query);

			var filters = query.GetFilters();
			var collection = GetCollection();
			var mongoCursor = filters == null ? collection.Find(x => true) : collection.Find(filters);

			var fields = query.GetFields();
			if (fields != null)
				mongoCursor.Project(fields);

			var mongoSortBy = query.GetSortOrder();
			if (mongoSortBy != null)
				mongoCursor.Sort(mongoSortBy);

			if (query.GetSkip() > 0)
				mongoCursor.Skip(query.GetSkip());

			if (query.GetTake() > 0)
				mongoCursor.Limit(query.GetTake());

			return mongoCursor.ToListAsync();
		}

		public Task<List<T>> QueryAsync(Action<IQueryBuilder<T>> queryManipulator)
		{
			throw new NotImplementedException();
		}

		public Task<List<T>> QueryAllAsync()
		{
			return GetCollection().Find(x => true).ToListAsync();
		}

		public Task<long> CountAsync(Action<IQueryBuilder<T>> queryManipulator = null)
		{
			throw new NotImplementedException();
		}

		public Task<T> GetByIdAsync(T entity)
		{
			return GetCollection()
				.Find(Builders<T>.Filter.Eq(x => x.Id, entity.Id)).ToListAsync()
				.ContinueWith(t => t.Result.SingleOrDefault());
		}

		public Task<long> CountAsync(Action<ITypeQueryBuilder<T>> queryManipulator)
		{
			var query = new MongoDbQueryBuilder<T>();
			queryManipulator?.Invoke(query);
			return GetCollection().CountAsync(query.GetFilters());
		}

		public Task AddAsync(T t)
		{
			return GetCollection().InsertOneAsync(t);
		}

		public Task SaveAsync(T t)
		{
			return GetCollection().ReplaceOneAsync(Builders<T>.Filter.Eq(s => s.Id, t.Id), t, new UpdateOptions { IsUpsert = true });
		}

		public Task UpdateAsync(T t)
		{
			return GetCollection().ReplaceOneAsync(Builders<T>.Filter.Eq(s => s.Id, t.Id), t, new UpdateOptions { IsUpsert = false });
		}

		public Task DeleteAsync(T t)
		{
			return GetCollection().DeleteOneAsync(Builders<T>.Filter.Eq(s => s.Id, t.Id));
		}

		public Task DeleteAsync(IList<T> ts)
		{
			var ids = ts.Select(x => x.Id).ToList();

			return GetCollection().DeleteManyAsync(Builders<T>.Filter.In(x => x.Id, ids));
		}

		public Task<List<T>> GetByFieldAsync<TKey>(Expression<Func<T, TKey>> selector, TKey key)
		{
			return GetCollection().Find(Builders<T>.Filter.Eq(selector, key)).ToListAsync();
		}

		public Task<List<T>> GetByFieldAsync<TKey>(Expression<Func<T, TKey>> selector, IEnumerable<TKey> ids)
		{
			return GetCollection().Find(Builders<T>.Filter.In(selector, ids)).ToListAsync();
		}
	}
}