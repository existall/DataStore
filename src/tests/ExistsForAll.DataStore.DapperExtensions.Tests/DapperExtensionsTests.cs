using System.Data;
using ExistAll.DataStore.Sql;
using ExistsForAll.DapperExtensions;
using NSubstitute;
using Xunit;

namespace ExistsForAll.DataStore.DapperExtensions.Tests
{
	public class DapperExtensionsTests
	{
		[Fact]
		public void X()
		{
			var dapperImplementor = Substitute.For<IDapperImplementor>();
			var connectionProvider = Substitute.For<IConnectionProvider>();
			var connection = Substitute.For<IDbConnection>();

			connectionProvider.GetConnection()
				.Returns(connection);

			var sut = new DapperDataStore<Person>(dapperImplementor, connectionProvider);

			var preson = new Person
			{
				Age = 10,
				Id = 1,
				Name = "jndbjncv"
			};

			sut.Count(a => a.Equal(v => v.Id,1));
		}
	}

	public class Person
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
	}
}