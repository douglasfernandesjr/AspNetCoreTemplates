using CleanArchTemplate.Core.Entities.Base;

namespace CleanArchTemplate.Tests.Infra.EF.Mocks.Entities
{
	public class EntidadeGenericaA : EntityBase
	{
		public EntidadeGenericaA()
		{
				
		}

		public EntidadeGenericaA(string nome, double valor)
		{
			Nome = nome;
			Valor = valor;
		}

		public int Id { get; set; }

		public string Nome { get; set; }

		public double Valor { get; set; }
	}
}
