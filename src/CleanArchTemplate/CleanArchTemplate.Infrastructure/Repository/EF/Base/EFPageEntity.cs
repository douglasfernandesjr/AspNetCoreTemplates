using CleanArchTemplate.Core.Interfaces.Data;
using System.Collections.Generic;

namespace CleanArchTemplate.Infrastructure.Repository.EF.Base
{
	public class EFPageEntity<T> : IPageEntity<T>
	where T : IEntity
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public IEnumerable<T> Data { get; set; }
		public int? TotalItens { get; set; }
		public int? TotalPages { get; set; }
		public bool LastPage { get; set; }
	}
}
