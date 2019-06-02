using System.Collections.Generic;

namespace CleanArchTemplate.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IRepository<T>
		where T : IEntity
	{
		void Insert(T model);

		void Insert(IEnumerable<T> models);

		void Update(T model);

		void Update(IEnumerable<T> models);

		void Delete(T model);

		void Delete(IEnumerable<T> models);

		void DeleteById<TProp>(TProp key);

		void DeleteByIds<TProp>(IEnumerable<TProp> keys);
	}
}