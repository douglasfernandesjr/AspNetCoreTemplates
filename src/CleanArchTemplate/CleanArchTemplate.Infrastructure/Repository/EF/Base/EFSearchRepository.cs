using CleanArchTemplate.Core.Entities.Base;
using CleanArchTemplate.Core.Interfaces;
using CleanArchTemplate.Core.Interfaces.Data;
using CleanArchTemplate.Core.Interfaces.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchTemplate.Infrastructure.Repository.EF.Base
{
	public class EFSearchRepository<T> : ICustomSearchExecuter<T>
		where T : EntityBase, new()
	{
		private DbContext _dbContext;

		public EFSearchRepository(DbContext db)
		{
			_dbContext = db;
		}

		#region Executes
		public int ExecuteCount()
		{
			throw new NotImplementedException();
		}

		public IPageEntity<T> ExecutePagedSearch(uint pageIndex, uint pageSize)
		{
			throw new NotImplementedException();
		}

		public IPageEntity<T> ExecutePagedWithCount(uint pageIndex, uint pageSize)
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
