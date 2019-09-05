using CleanArchTemplate.Core.Interfaces.Data;

namespace CleanArchTemplate.Core.Interfaces.DataAccess
{
	public interface IRepository<T> : IRepositoryCRUD<T>, ICustomQueryRepository<T>
		where T : IEntity
	{
	}
}
