using CleanArchTemplate.Core.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CleanArchTemplate.Core.Interfaces.DataAccess
{
	public interface ICustomSearchOrder<T>
		where T : IEntity
	{
		ICustomSearchExecuter<T> OrderByAsc<TProp>(Expression<Func<T, TProp>> orderByAsc);

		ICustomSearchExecuter<T> OrderByDesc<TProp>(Expression<Func<T, TProp>> orderByDesc);
	}
}
