using System.Collections;
using DapperExtensions;

namespace ExistsForAll.DataStore.DapperExtensions
{
	public interface IInPredicate : IPredicate
	{
		ICollection Collection { get; set; }
		bool Not { get; set; }
	}
}