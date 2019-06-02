using System.Collections.Generic;

namespace CleanArchTemplate.Core.Interfaces
{
	public interface IRepository<T>
		where T : IEntity
	{
		void Insert(T model);

		void Insert(IEnumerable<T> models);

		void Update(T model);

		void Update(IEnumerable<T> models);

		void Delete(T model);

		void Delete(IEnumerable<T> models);

		void Delete<TProp>(TProp key);

		void Delete<TProp>(IEnumerable<TProp> keys);
	}
}