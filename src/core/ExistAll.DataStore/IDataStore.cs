using System;
using System.Collections.Generic;

namespace ExistAll.DataStore
{
	public interface IDataStore<T> where T : class
	{
		IEnumerable<T> Query(Action<IQueryBuilder<T>> queryManipulator);
		IEnumerable<T> QueryAll();

		long Count(Action<IQueryBuilder<T>> queryManipulator = null);

		T GetById(T id);
		void Add(T t);
		void Save(T t);
		void Update(T t);
		void Delete(T t);
	}
}