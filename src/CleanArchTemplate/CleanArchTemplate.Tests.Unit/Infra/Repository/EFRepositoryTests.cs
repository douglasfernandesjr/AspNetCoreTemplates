using CleanArchTemplate.Core.Interfaces;
using CleanArchTemplate.Infrastructure.Repository.EF.Base;
using CleanArchTemplate.Tests.Unit.Infra.Repository.Config;
using CleanArchTemplate.Tests.Unit.Infra.Repository.Config.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CleanArchTemplate.Tests.Unit.Infra.Repository
{
	public class EFRepositoryTests : IClassFixture<InMemoryTestFixture>
	{
		private readonly InMemoryTestFixture _fixture;

		public EFRepositoryTests(InMemoryTestFixture fixture)
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

		#region Testes Manipulando 1 entidade

		[Fact]
		public void DeveInserirUm()
		{
			var repo = GetRepo();

			var produto = new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA);

			repo.Insert(produto);

			Assert.True(produto.Id > 0);
		}

		[Fact]
		public void DeveInserirAtualizarDeletarUm()
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
		public void DeveInserirEDeletarUm()
		{
			var repo = GetRepo();

			var produto = new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA);

			repo.Insert(produto);

			var produtoSelectInsert = GetById(produto);
			Assert.NotNull(produtoSelectInsert);

			repo = GetRepo();

			var produtoSelectToDelete = repo.FindById(produtoSelectInsert.Id);

			repo.Delete(produtoSelectToDelete);

			var produtoSelectDelete = GetById(produtoSelectToDelete);

			Assert.Null(produtoSelectDelete);
		}

		[Fact]
		public void DeveNaoDeletarInvalidoUm()
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

		}

		[Fact]
		public void DeveIgnorarUm()
		{
			var repo = GetRepo();

			EntidadeGenericaA produto = null;

			repo.Insert(produto);
			Assert.Null(produto);

			repo.Update(produto);
			Assert.Null(produto);

			repo.Delete(produto);
			Assert.Null(produto);

			var find = repo.FindById<int>(-1);
			Assert.Null(find);
		}

		#endregion Testes Manipulando 1 entidade

		#region Testes Manipulando multiplas entidade

		[Fact]
		public void DeveInserirVarios()
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

		[Fact]
		public void DeveInserirVariosIgnorarNulls()
		{
			var repo = GetRepo();

			var produtos = new List<EntidadeGenericaA>();
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
			produtos.Add(null);
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoA));
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoB));

			repo.Insert(produtos);

			Assert.True(produtos[0].Id > 0);
			Assert.Null(produtos[1]);
			Assert.True(produtos[2].Id > 0);
			Assert.True(produtos[3].Id > 0);
		}

		[Fact]
		public void DeveInserirAtualizarVariosIgnorarNulls()
		{
			var repo = GetRepo();

			var produtos = new List<EntidadeGenericaA>();
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
			produtos.Add(null);
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoA));
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoB));

			repo.Insert(produtos);
			produtos[0].Nome = MockValues.NomeGenericoB;
			produtos[2].Nome = MockValues.NomeGenericoA;
			produtos[3].Nome = MockValues.NomeGenericoA;


			repo.Update(produtos);

			Assert.True(produtos[0].Id > 0);
			Assert.Null(produtos[1]);
			Assert.True(produtos[2].Id > 0);
			Assert.True(produtos[3].Id > 0);

			repo = GetRepo();

			var lista = repo.FindByIds(new int[] { produtos[0].Id, produtos[2].Id, produtos[3].Id }).ToList();

			Assert.True(lista[0].Nome == MockValues.NomeGenericoB);
			Assert.True(lista[1].Nome == MockValues.NomeGenericoA);
			Assert.True(lista[2].Nome == MockValues.NomeGenericoA);
		}

		[Fact]
		public void DeveInserirDeletarVariosIgnorarNulls()
		{
			var repo = GetRepo();

			var produtos = new List<EntidadeGenericaA>();
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
			produtos.Add(null);
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoA));
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoB));

			repo.Insert(produtos);

			repo.Delete(produtos);

			repo = GetRepo();

			var lista = repo.FindByIds(new int[] { produtos[0].Id, produtos[2].Id, produtos[3].Id }).ToList();

			Assert.Empty(lista);
		}


		[Fact]
		public void DeveInserirAtualizarDeletarVarios()
		{
			var repo = GetRepo();

			var produtos = new List<EntidadeGenericaA>();
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoB));

			repo.Insert(produtos);

			Assert.True(produtos[0].Id > 0);
			Assert.True(produtos[1].Id > 0);

			var produtoSelectInsert0 = GetById(produtos[0]);
			var produtoSelectInsert1 = GetById(produtos[1]);
			Assert.NotNull(produtoSelectInsert0);
			Assert.NotNull(produtoSelectInsert1);

			produtos[0].Nome = MockValues.NomeGenericoB;
			produtos[0].Valor = MockValues.ValorGenericoB;

			produtos[1].Nome = MockValues.NomeGenericoA;
			produtos[1].Valor = MockValues.ValorGenericoA;

			repo.Update(produtos);

			var produtoSelectUpdate0 = GetById(produtos[0]);
			var produtoSelectUpdate1 = GetById(produtos[1]);

			Assert.Equal(produtoSelectUpdate0.Nome, MockValues.NomeGenericoB);
			Assert.Equal(produtoSelectUpdate0.Valor, MockValues.ValorGenericoB);
			Assert.Equal(produtoSelectUpdate1.Nome, MockValues.NomeGenericoA);
			Assert.Equal(produtoSelectUpdate1.Valor, MockValues.ValorGenericoA);

			repo.Delete(produtos);

			var produtoSelectDelete0 = GetById(produtos[0]);
			var produtoSelectDelete1 = GetById(produtos[1]);

			Assert.Null(produtoSelectDelete0);
			Assert.Null(produtoSelectDelete1);
		}

		[Fact]
		public void DeveInserirAtualizarDeletarFindVarios()
		{
			var repo = GetRepo();

			var produtos = new List<EntidadeGenericaA>();
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoB));

			repo.Insert(produtos);

			Assert.True(produtos[0].Id > 0);
			Assert.True(produtos[1].Id > 0);

			var produtoSelectInsert0 = repo.FindById(produtos[0].Id);
			var produtoSelectInsert1 = repo.FindById(produtos[1].Id);
			Assert.NotNull(produtoSelectInsert0);
			Assert.NotNull(produtoSelectInsert1);

			produtos[0].Nome = MockValues.NomeGenericoB;
			produtos[0].Valor = MockValues.ValorGenericoB;

			produtos[1].Nome = MockValues.NomeGenericoA;
			produtos[1].Valor = MockValues.ValorGenericoA;

			repo.Update(produtos);

			var produtoSelectUpdate0 = repo.FindById(produtos[0].Id);
			var produtoSelectUpdate1 = repo.FindById(produtos[1].Id);

			Assert.Equal(produtoSelectUpdate0.Nome, MockValues.NomeGenericoB);
			Assert.Equal(produtoSelectUpdate0.Valor, MockValues.ValorGenericoB);
			Assert.Equal(produtoSelectUpdate1.Nome, MockValues.NomeGenericoA);
			Assert.Equal(produtoSelectUpdate1.Valor, MockValues.ValorGenericoA);

			repo.Delete(produtos);

			var produtoSelectDelete0 = repo.FindById(produtos[0].Id);
			var produtoSelectDelete1 = repo.FindById(produtos[1].Id);

			Assert.Null(produtoSelectDelete0);
			Assert.Null(produtoSelectDelete1);
		}

		[Fact]
		public void DeveInserirEDeletarVarios()
		{
			var repo = GetRepo();

			var produtos = new List<EntidadeGenericaA>();
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
			produtos.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoB));

			repo.Insert(produtos);

			var produtoSelectInsert = repo.FindByIds(new int[] { produtos[0].Id, produtos[1].Id }).ToArray();

			repo.Delete(new EntidadeGenericaA[] { produtoSelectInsert[0], produtoSelectInsert[1] });

			var produtoSelectDelete0 = repo.FindById(produtos[0].Id);
			var produtoSelectDelete1 = GetById(produtos[1]);

			Assert.Null(produtoSelectDelete0);
			Assert.Null(produtoSelectDelete1);
		}


		[Fact]
		public void DeveIgnorarFindVarios()
		{
			var repo = GetRepo();

			var listaTeste1 = repo.FindByIds(new int[] { 12345, 212345 });
			Assert.Empty(listaTeste1);

			int[] lista = null;

			var listaTeste2 = repo.FindByIds(lista);
			Assert.Empty(listaTeste2);

			var produto = new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA);

			repo.Insert(produto);

			var listaTeste3 = repo.FindByIds(new int[] { produto.Id, 4434342 }).ToArray();
			Assert.NotEmpty(listaTeste3);
			Assert.Single(listaTeste3);

		}
		#endregion Testes Manipulando multiplas entidade
	}
}
