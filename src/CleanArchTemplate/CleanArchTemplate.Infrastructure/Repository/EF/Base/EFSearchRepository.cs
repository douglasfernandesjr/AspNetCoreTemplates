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
		private readonly DbContext _dbContext;

		private uint? _skip;
		private uint? _take;

		private List<string> _includesString;
		private List<Expression<Func<T, IEntity>>> __includesExpression;
		private List<Expression<Func<T, IEnumerable<IEntity>>>> _includesExpressionList;

		private Expression<Func<T, bool>> _whereExpression;

		private List<LambdaExpression> _orderByAscExpression;
		private List<LambdaExpression> _orderByDescExpression;

		

		public EFSearchRepository(DbContext db)
		{
			_dbContext = db;
		}



		public ICustomSearchExecuter<T> All()
		{
			return this;
		}

		public ICustomSearchExecuter<T> Where(Expression<Func<T, bool>> filterExpression)
		{
			_whereExpression = filterExpression;
			return this;
		}


		public ICustomSearchExecuter<T> IncludeEntity(string path)
		{
			if (_includesString == null)
				_includesString = new List<string>();

			_includesString.Add(path);

			return this;
		}

		public ICustomSearchExecuter<T> IncludeEntity(Expression<Func<T, IEntity>> path)
		{
			if (__includesExpression == null)
				__includesExpression = new List<Expression<Func<T, IEntity>>>();

			__includesExpression.Add(path);

			return this;
		}


		

		ICustomSearchExecuter<T> ICustomSearchExecuter<T>.OrderByAsc<TProp>(Expression<Func<T, TProp>> orderByAsc)
		{
			if (_orderByAscExpression == null)
				_orderByAscExpression = new List<LambdaExpression>();

			_orderByAscExpression.Add(orderByAsc);
			return this;
		}

		ICustomSearchExecuter<T> ICustomSearchExecuter<T>.OrderByDesc<TProp>(Expression<Func<T, TProp>> orderByDesc)
		{
			if (_orderByDescExpression == null)
				_orderByDescExpression = new List<LambdaExpression>();

			_orderByDescExpression.Add(orderByDesc);
			return this;
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
