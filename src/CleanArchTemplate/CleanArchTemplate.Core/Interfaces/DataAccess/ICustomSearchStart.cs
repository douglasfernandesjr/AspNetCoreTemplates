using CleanArchTemplate.Core.Interfaces.Data;
using System;
using System.Linq.Expressions;

namespace CleanArchTemplate.Core.Interfaces.DataAccess
{
	public interface ICustomSearchStart<T>
		where T : IEntity
	{
		ICustomSearchExecuter<T> All();

		ICustomSearchExecuter<T> Where(Expression<Func<T, bool>> filterExpression);
	}
}