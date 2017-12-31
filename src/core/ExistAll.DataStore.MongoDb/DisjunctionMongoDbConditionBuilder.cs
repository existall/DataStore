using MongoDB.Driver;

namespace ExistAll.DataStore.MongoDb
{
	internal class DisjunctionMongoDbConditionBuilder<T> : MongoDbConditionBuilder<T>
	{
		protected override FilterDefinition<T> CombineFilter(FilterDefinition<T> aggregatedFilter, FilterDefinition<T> filter)
		{
			return aggregatedFilter | filter;
		}
	}
}