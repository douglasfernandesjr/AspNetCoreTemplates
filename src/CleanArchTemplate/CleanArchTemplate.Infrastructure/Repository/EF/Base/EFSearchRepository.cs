using CleanArchTemplate.Core.Entities.Base;
using CleanArchTemplate.Core.Interfaces;
using CleanArchTemplate.Core.Interfaces.Data;
using CleanArchTemplate.Core.Interfaces.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CleanArchTemplate.Infrastructure.Repository.EF.Base
{
	public class EFSearchRepository<T> : ICustomSearch<T>, ICustomSearchExecuter<T>
		where T : EntityBase, new()
	{
		private DbContext _dbContext;

		public EFSearchRepository(DbContext db)
		{
			_dbContext = db;
		}

		public ICustomSearchExecuter<T> All(Expression<Func<T, bool>> filterExpression)
		{
			throw new NotImplementedException();
		}

		public ICustomSearchExecuter<T> All()
		{
			throw new NotImplementedException();
		}

	

		public ICustomSearchExecuter<T> IncludeEntity(string path)
		{
			throw new NotImplementedException();
		}

		public ICustomSearchExecuter<T> IncludeEntity(Expression<Func<T, IEntity>> path)
		{
			throw new NotImplementedException();
		}


		public ICustomSearchExecuter<T> Where(Expression<Func<T, bool>> filterExpression)
		{
			throw new NotImplementedException();
		}

		ICustomSearchExecuter<T> ICustomSearchExecuter<T>.OrderByAsc<TProp>(Expression<Func<T, TProp>> orderByAsc)
		{
			throw new NotImplementedException();
		}

		ICustomSearchExecuter<T> ICustomSearchExecuter<T>.OrderByDesc<TProp>(Expression<Func<T, TProp>> orderByDesc)
		{
			throw new NotImplementedException();
		}


		#region Execute
		public int ExecuteCount()
		{
			throw new NotImplementedException();
		}

		public IPageEntity<T> ExecutePagedSearch(uint pageIndex, uint pageSize)
		{
			throw new NotImplementedException();
		}

		public IPageEntity<T> ExecutePagedSearchWithCount(uint pageIndex, uint pageSize)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<T> ExecuteSearch()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
