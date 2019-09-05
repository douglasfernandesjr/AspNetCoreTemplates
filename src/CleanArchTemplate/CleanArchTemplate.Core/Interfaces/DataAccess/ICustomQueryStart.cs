using CleanArchTemplate.Core.Interfaces.Data;
using System;
using System.Linq.Expressions;

namespace CleanArchTemplate.Core.Interfaces.DataAccess
{
	public interface ICustomQueryStart<T>
		where T : IEntity
	{
		ICustomQueryExecuter<T> All();

		ICustomQueryExecuter<T> Where(Expression<Func<T, bool>> filterExpression);
	}
}