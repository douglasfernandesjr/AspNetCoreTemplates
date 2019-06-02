using CleanArchTemplate.Core.Entities.Base;

namespace CleanArchTemplate.Tests.Infra.EF.Entities
{
	public class Produto : EntityBase
	{
		public int Id { get; set; }

		public string Nome { get; set; }

		public double Valor { get; set; }
	}
}
