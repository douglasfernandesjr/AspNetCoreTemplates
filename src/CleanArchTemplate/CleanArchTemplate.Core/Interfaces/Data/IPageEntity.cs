using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchTemplate.Core.Interfaces.Data
{
	public interface IPageEntity<T>
		where T : IEntity
	{
		int PageIndex { get; set; }
		int PageSize { get; set; }

		IEnumerable<T> Data { get; set; }

		int? TotalItens { get; set; }

		int? TotalPages { get; set; }

		bool LastPage { get; set; }
	}
}
