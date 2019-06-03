using System.Collections.Generic;

namespace CleanArchTemplate.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IRepositoryPaging<T> : IRepository<T>
		where T : IEntity
	{
		IPagingEntity<T> Paging<TProp>();
	}
}