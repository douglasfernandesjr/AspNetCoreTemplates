using CleanArchTemplate.Core.Interfaces;
using CleanArchTemplate.Infrastructure.Repository.EF.Base;
using CleanArchTemplate.Tests.Infra.EF.Mocks;
using CleanArchTemplate.Tests.Infra.EF.Mocks.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CleanArchTemplate.Tests.Infra
{
	public class IRepositoryTests : IClassFixture<InMemoryTestFixture>
	{
		private readonly InMemoryTestFixture _fixture;

		public IRepositoryTests(InMemoryTestFixture fixture)
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
			using (var db = _fixture.Context())
			{
				return db.EntidadeGenericaA.Where(x => x.Id == id).FirstOrDefault();
			}
		}

		private IRepository<EntidadeGenericaA> GetRepo()
		{
			return new Repository<EntidadeGenericaA>(_fixture.Context());
		}

		#region  Testes Manipulando 1 entidade
		[Fact]
		public void DeveCriarNovo()
		{
			var repo = GetRepo();

			var produto = new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA);

			repo.Insert(produto);

			Assert.True(produto.Id > 0);
		}

		[Fact]
		public void DeveInserirAtualizarDeletarUM()
		{
			var repo = GetRepo();

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
			var repo = GetRepo();

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
			var repo = GetRepo();

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

		#endregion

		#region  Testes Manipulando multiplas entidade
		[Fact]
		public void DeveCriarNovos()
		{
			var repo = GetRepo();

			var produtos = new List<EntidadeGenericaA>();
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoB));
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoA));
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoB));

			repo.Insert(produtos);

			Assert.True(produtos[0].Id > 0);
			Assert.True(produtos[1].Id > 0);
			Assert.True(produtos[2].Id > 0);
			Assert.True(produtos[3].Id > 0);
		}
		#endregion
	}
}