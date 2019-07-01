using CleanArchTemplate.Core.Interfaces.Data;
using System.Collections.Generic;

namespace CleanArchTemplate.Core.Interfaces.DataAccess
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICustomSearchExecuter<T> 
		where T : IEntity
	{
		int ExecuteCount();

		IEnumerable<T> ExecuteSearch();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pageIndex">Starts in 0</param>
		/// <param name="pageSize">Starts in 1</param>
		/// <returns></returns>
		IPageEntity<T> ExecutePagedSearch(uint pageIndex, uint pageSize);
		IPageEntity<T> ExecutePagedWithCount(uint pageIndex, uint pageSize);
	}
}