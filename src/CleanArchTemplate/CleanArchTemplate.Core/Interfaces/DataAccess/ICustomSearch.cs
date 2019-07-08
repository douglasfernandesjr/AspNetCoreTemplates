using CleanArchTemplate.Core.Interfaces.Data;

namespace CleanArchTemplate.Core.Interfaces.DataAccess
{
	public interface ICustomSearch<T> : ICustomSearchStart<T>, ICustomSearchExecuter<T>
		where T : IEntity
	{
	}
}