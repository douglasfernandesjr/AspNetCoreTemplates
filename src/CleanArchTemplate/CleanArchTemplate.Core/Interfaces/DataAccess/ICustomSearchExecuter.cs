using CleanArchTemplate.Core.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanArchTemplate.Core.Interfaces.DataAccess
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICustomSearchExecuter<T> 
		where T : IEntity
	{
		ICustomSearchExecuter<T> IncludeEntity(string path);

		ICustomSearchExecuter<T> IncludeEntity(Expression<Func<T, IEntity>> path);

		ICustomSearchExecuter<T> OrderByAsc<TProp>(Expression<Func<T, TProp>> orderByAsc);

		ICustomSearchExecuter<T> OrderByDesc<TProp>(Expression<Func<T, TProp>> orderByDesc);

		int ExecuteCount();

		IEnumerable<T> ExecuteSearch();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pageIndex">Starts in 0</param>
		/// <param name="pageSize">Starts in 1</param>
		/// <returns></returns>
		IPageEntity<T> ExecutePagedSearch(uint pageIndex, uint pageSize);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pageIndex">Starts in 0</param>
		/// <param name="pageSize">Starts in 1</param>
		/// <returns></returns>
		IPageEntity<T> ExecutePagedSearchWithCount(uint pageIndex, uint pageSize);
	}
}