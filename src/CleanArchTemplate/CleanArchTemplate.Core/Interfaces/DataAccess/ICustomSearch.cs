using CleanArchTemplate.Core.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CleanArchTemplate.Core.Interfaces.DataAccess
{
	public interface ICustomSearch<T>
		where T : IEntity
	{
		ICustomSearchExecuter<T> All(Expression<Func<T, bool>> filterExpression)
		ICustomSearchExecuter<T> Where(Expression<Func<T, bool>> filterExpression)

	}
}
