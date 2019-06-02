using CleanArchTemplate.Infrastructure.Repository.EF.Base;
using CleanArchTemplate.Tests.Infra.EF;
using CleanArchTemplate.Tests.Infra.EF.Entities;
using System;
using Xunit;

namespace CleanArchTemplate.Tests.Infra
{
	public class RepositoryTests : IClassFixture<InMemoryTestFixture>
	{
		private readonly InMemoryTestFixture _fixture;
		public RepositoryTests(InMemoryTestFixture fixture)
		{
			_fixture = fixture;
		}


		[Fact]
		public void DeveCriarNovo()
		{
			var repo = new Repository<Produto>(_fixture.Context);

			var produto = new Produto() { Nome = "Produto", Valor = 123 };

			repo.Insert(produto);

			Assert.Equal(1, produto.Id);
		}
	}
}
