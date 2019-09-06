using CleanArchTemplate.Core.Interfaces.Data;

namespace CleanArchTemplate.Core.Interfaces.DataAccess
{
	public interface ICustomQuery<T> : ICustomQueryStart<T>, ICustomQueryExecuter<T>
		where T : IEntity
	{
	}
}