using CleanArchTemplate.Core.Interfaces;
using CleanArchTemplate.Infrastructure.Repository.EF.Base;
using CleanArchTemplate.Tests.Infra.EF.Mocks;
using CleanArchTemplate.Tests.Infra.EF.Mocks.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

		/// <summary>
		/// É um bug conhecido no In Memory Database, um novo context deve ser aberto para acessar os dados salvos inicialmente
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private EntidadeGenericaA GetById(EntidadeGenericaA model)
		{
			return GetById(model.Id);
		}

		private EntidadeGenericaA GetById(int id)
		{
			using (var db = _fixture.NewContext())
			{
				return db.EntidadeGenericaA.Where(x => x.Id == id).FirstOrDefault();
			}
		}

		[Fact]
		public void DeveCriarNovo()
		{
			IRepository<EntidadeGenericaA> repo = new Repository<EntidadeGenericaA>(_fixture.Context);

			var produto = new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA);

			repo.Insert(produto);

			Assert.True(produto.Id > 0);
		}

		[Fact]
		public void DeveInserirAtualizarDeletarUM()
		{
			IRepository<EntidadeGenericaA> repo = new Repository<EntidadeGenericaA>(_fixture.Context);

			var produto = new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA);

			repo.Insert(produto);

			Assert.True(produto.Id > 0);

			var produtoSelectInsert = GetById(produto);
			Assert.NotNull(produtoSelectInsert);

			produto.Nome = MockValues.NomeGenericoB;
			produto.Valor = MockValues.ValorGenericoB;

			repo.Update(produto);

			var produtoSelectUpdate = GetById(produto);

			Assert.Equal(produtoSelectUpdate.Nome, MockValues.NomeGenericoB);
			Assert.Equal(produtoSelectUpdate.Valor, MockValues.ValorGenericoB);

			repo.Delete(produto);

			var produtoSelectDelete = GetById(produto);

			Assert.Null(produtoSelectDelete);
		}

		[Fact]
		public void DeveInserirEDeletarUM()
		{
			IRepository<EntidadeGenericaA> repo = new Repository<EntidadeGenericaA>(_fixture.Context);

			var produto = new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA);

			repo.Insert(produto);

			var produtoSelectInsert = GetById(produto);
			Assert.NotNull(produtoSelectInsert);

			repo.Delete(produtoSelectInsert.Id);

			var produtoSelectDelete = GetById(produto);

			Assert.Null(produtoSelectDelete);
		}

		[Fact]
		public void NaoDeveDeletarInvalido()
		{
			IRepository<EntidadeGenericaA> repo = new Repository<EntidadeGenericaA>(_fixture.Context);

			var produto = new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA);
			produto.Id = 12345;

			Assert.Throws<DbUpdateConcurrencyException>(
				() =>
				{
					repo.Delete(produto);
				}
			);

			repo.Delete(1234);
			var produtoSelectDelete = GetById(1234);
			Assert.Null(produtoSelectDelete);
		}
	}
}