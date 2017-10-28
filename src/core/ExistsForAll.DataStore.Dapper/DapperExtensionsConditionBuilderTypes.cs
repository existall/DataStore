namespace ExistsForAll.DataStore.DapperExtensions
{

	internal class Person
	{
		public string Name { get; set; }
		public int Age { get; set; }
	}

	internal class TestDataStore : DapperExtensionsDataStore<Person>
	{
		private readonly IDbConnectionProvider _connectionProvider;

		public TestDataStore(IDbConnectionProvider connectionProvider)
			: base(connectionProvider)
		{
			_connectionProvider = connectionProvider;


			QueryAsync(x => x.Where(y => y.Equal(z => z.Age, 2))
			.Take(1));

		}
	}
}