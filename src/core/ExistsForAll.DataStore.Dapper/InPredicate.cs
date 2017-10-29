using System.Collections;
using System.Collections.Generic;
using DapperExtensions;
using DapperExtensions.Sql;

namespace ExistsForAll.DataStore.DapperExtensions
{
	public class InPredicate<T> : BasePredicate, IInPredicate
		where T : class
	{
		public ICollection Collection { get; }
		public bool Not { get; set; }

		public InPredicate(ICollection collection, string propertyName, bool isNot = false)
		{
			PropertyName = propertyName;
			Collection = collection;
			Not = isNot;
		}

		public override string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
		{
			var columnName = GetColumnName(typeof(T), sqlGenerator, PropertyName);

			var @params = new List<string>();

			foreach (var item in Collection)
			{
				@params.Add(parameters.SetParameterName(PropertyName, item, sqlGenerator.Configuration.Dialect.ParameterPrefix));
			}

			var commaDelimited = string.Join(",", @params);

			return $@"({columnName} {GetIsNotStatement()} IN ({commaDelimited}))";
		}

		private string GetIsNotStatement()
		{
			return Not ? "NOT " : string.Empty;
		}
	}
}