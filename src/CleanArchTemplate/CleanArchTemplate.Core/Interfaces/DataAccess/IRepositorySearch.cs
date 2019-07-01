using CleanArchTemplate.Core.Interfaces.Data;
using System.Collections.Generic;

namespace CleanArchTemplate.Core.Interfaces.DataAccess
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IRepositorySearch<T> : IRepository<T>
		where T : IEntity
	{
		ICustomSearch<T> NewSearch();
	}
}