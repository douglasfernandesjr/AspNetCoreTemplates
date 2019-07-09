using CleanArchTemplate.Core.Entities.Base;

namespace CleanArchTemplate.Tests.Unit.Infra.Repository.Config.Entities
{
	public class EntidadeGenericaC : EntityBase
	{
		public EntidadeGenericaC()
		{
		}

		public EntidadeGenericaC(string nome, double valor)
		{
			Nome = nome;
			Valor = valor;
		}

		public int Id { get; set; }

		public string Nome { get; set; }

		public double Valor { get; set; }
		public int EntidadeAId { get; set; }
		public virtual EntidadeGenericaA EntidadeA { get; set; }
	}
}