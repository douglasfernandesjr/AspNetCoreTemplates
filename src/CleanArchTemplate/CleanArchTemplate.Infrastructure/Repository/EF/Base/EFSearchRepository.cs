using CleanArchTemplate.Core.Entities.Base;
using CleanArchTemplate.Core.Interfaces.Data;
using CleanArchTemplate.Core.Interfaces.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CleanArchTemplate.Infrastructure.Repository.EF.Base
{
	public class EFSearchRepository<T> : ICustomSearch<T>
		where T : EntityBase, new()
	{
		private readonly DbContext _dbContext;

		private List<string> _includesString;
		private List<Expression<Func<T, IEntity>>> __includesExpression;

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

		public ICustomSearchExecuter<T> OrderByAsc<TProp>(Expression<Func<T, TProp>> orderByAsc)
		{
			if (_orderByAscExpression == null)
				_orderByAscExpression = new List<LambdaExpression>();

			_orderByAscExpression.Add(orderByAsc);
			return this;
		}

		public ICustomSearchExecuter<T> OrderByDesc<TProp>(Expression<Func<T, TProp>> orderByDesc)
		{
			if (_orderByDescExpression == null)
				_orderByDescExpression = new List<LambdaExpression>();

			_orderByDescExpression.Add(orderByDesc);
			return this;
		}

		#region Execute

		public int ExecuteCount() => CountQuery().Count();

		public async Task<int> ExecuteCountAsync() => await CountQuery().CountAsync();

		public IEnumerable<T> ExecuteSearch() => SearchQuery().ToList();

		public async Task<IEnumerable<T>> ExecuteSearchAsync() => await SearchQuery().ToListAsync();

		public IPageEntity<T> ExecutePagedSearch(int pageIndex, int pageSize)
			=> PageEntity(pageIndex, pageSize, SmartSearchQuery(pageIndex, pageSize).ToList());

		public async Task<IPageEntity<T>> ExecutePagedSearchAsync(int pageIndex, int pageSize)
			=> PageEntity(pageIndex, pageSize, await SmartSearchQuery(pageIndex, pageSize).ToListAsync());

		public IPageEntity<T> ExecutePagedSearchWithCount(int pageIndex, int pageSize)
		{
			int count = CountQuery().Count();
			var result = SearchQuery(pageIndex, pageSize).ToList();

			return PageEntity(pageIndex, pageSize, result, count);
		}

		public async Task<IPageEntity<T>> ExecutePagedSearchWithCountAsync(int pageIndex, int pageSize)
		{
			int count = await CountQuery().CountAsync();

			var result = await SearchQuery(pageIndex, pageSize).ToListAsync();

			return PageEntity(pageIndex, pageSize, result, count);
		}

		private EFPageEntity<T> PageEntity(int pageIndex, int pageSize, List<T> result, int? count = null)
		{
			var page = new EFPageEntity<T>
			{
				PageIndex = pageIndex,
				PageSize = pageSize
			};

			int length = result.Count();

			if (!count.HasValue)
			{
				if (length == pageSize + 1)
				{
					page.Data = result.Take(pageSize);
					page.LastPage = false;
				}
				else
				{
					page.Data = result;
					page.LastPage = true;
				}
			}
			else
			{
				page.Data = result;
				page.TotalItens = count;
				page.TotalPages = (int)Math.Ceiling((double)count.Value / pageSize); ;
				page.LastPage = pageIndex + 1 == page.TotalPages;
			}

			return page;
		}

		#endregion Execute

		#region configs

		protected IQueryable<T> IncludesConfig(IQueryable<T> query)
		{
			if (_includesString != null && _includesString.Any())
			{
				foreach (string str in _includesString)
					query = query.Include(str);
			}

			if (__includesExpression != null && __includesExpression.Any())
			{
				foreach (var exp in __includesExpression)
					query = query.Include(exp);
			}
			return query;
		}

		protected IQueryable<T> FilterConfig(IQueryable<T> query)
		{
			if (_whereExpression != null)
			{
				query = query.Where(_whereExpression);
			}

			return query;
		}

		protected IQueryable<T> OrderByConfig(IQueryable<T> query)
		{
			if (_orderByAscExpression != null && _orderByAscExpression.Any())
			{
				foreach (var orderByItem in _orderByAscExpression)
					query = Queryable.OrderBy(query, (dynamic)orderByItem);
			}

			if (_orderByDescExpression != null && _orderByDescExpression.Any())
			{
				foreach (var orderByItem in _orderByDescExpression)
					query = Queryable.OrderByDescending(query, (dynamic)orderByItem);
			}
			return query;
		}

		#endregion configs

		#region queries

		public IQueryable<T> CountQuery()
		{
			var query = _dbContext.Set<T>().AsQueryable();
			return FilterConfig(query);
		}

		public IQueryable<T> SearchQuery()
		{
			var query = _dbContext.Set<T>().AsQueryable();
			return OrderByConfig(FilterConfig(IncludesConfig(query)));
		}

		public IQueryable<T> SmartSearchQuery(int pageIndex, int pageSize)
		{
			return SearchQuery().Skip(pageIndex * pageSize).Take(pageSize + 1);
		}

		public IQueryable<T> SearchQuery(int pageIndex, int pageSize)
		{
			return SearchQuery().Skip(pageIndex * pageSize).Take(pageSize + 1);
		}

		#endregion queries
	}
}