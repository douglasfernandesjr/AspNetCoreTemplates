﻿using CleanArchTemplate.Core.Entities.Base;

namespace CleanArchTemplate.Tests.Unit.Infra.Repository.Config.Entities
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