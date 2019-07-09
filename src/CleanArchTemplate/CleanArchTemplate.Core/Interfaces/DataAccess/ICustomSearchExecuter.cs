using CleanArchTemplate.Core.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

		int Count();

		IEnumerable<T> Search();

		/// <summary>
		/// Execute without count
		/// </summary>
		/// <param name="pageIndex">Starts in 0</param>
		/// <param name="pageSize">Starts in 1</param>
		/// <returns></returns>
		IPageEntity<T> SmartPagedSearch(int pageIndex, int pageSize);

		/// <summary>
		/// Execute sith count
		/// </summary>
		/// <param name="pageIndex">Starts in 0</param>
		/// <param name="pageSize">Starts in 1</param>
		/// <returns></returns>
		IPageEntity<T> PagedSearch(int pageIndex, int pageSize);

		Task<int> CountAsync();

		Task<IEnumerable<T>> SearchAsync();

		/// <summary>
		/// Execute without count
		/// </summary>
		/// <param name="pageIndex">Starts in 0</param>
		/// <param name="pageSize">Starts in 1</param>
		/// <returns></returns>
		Task<IPageEntity<T>> SmartPagedSearchAsync(int pageIndex, int pageSize);

		/// <summary>
		/// Execute sith count
		/// </summary>
		/// <param name="pageIndex">Starts in 0</param>
		/// <param name="pageSize">Starts in 1</param>
		/// <returns></returns>
		Task<IPageEntity<T>> PagedSearchAsync(int pageIndex, int pageSize);
	}
}