using CleanArchTemplate.Core.Interfaces.Data;
namespace CleanArchTemplate.Core.Interfaces.DataAccess
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICustomQueryRepository<T>
		where T : IEntity
	{
		ICustomQueryStart<T> Query();
	}
}