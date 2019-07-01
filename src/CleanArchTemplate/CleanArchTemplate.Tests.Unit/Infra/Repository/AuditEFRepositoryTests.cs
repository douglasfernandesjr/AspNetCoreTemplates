using CleanArchTemplate.Core.Interfaces;
using CleanArchTemplate.Core.Interfaces.DataAccess;
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
	public class AuditEFRepositoryTests : IClassFixture<InMemoryTestFixtureSQLLite>
	{
		private readonly InMemoryTestFixtureSQLLite _fixture;

		public AuditEFRepositoryTests(InMemoryTestFixtureSQLLite fixture)
		{
			_fixture = fixture;
		}

		private EFAuditRepository<EntidadeGenericaB> GetRepo()
		{
			return new EFAuditRepository<EntidadeGenericaB>(MockValues.GetMockUser(), _fixture.Context());
		}

		private EntidadeGenericaB GetById(EntidadeGenericaB model)
		{
			return GetById(model.Id);
		}

		private EntidadeGenericaB GetById(int id)
		{
			using (var db = _fixture.Context())
			{
				return db.EntidadeGenericaB.Where(x => x.Id == id).FirstOrDefault();
			}
		}

		[Fact]
		public void DeveInserirUm()
		{
			var repo = GetRepo();

			var produto = new EntidadeGenericaB(MockValues.NomeGenericoA, MockValues.ValorGenericoA);

			repo.Insert(produto);

			Assert.Equal(MockValues.MockUserName, produto.LoginInclusao);
			Assert.True(produto.DataHoraInclusao != null);
			Assert.False(produto.Excluido);
		}

		[Fact]
		public void DeveInserirAtualizarDeletarUm()
		{
			var repo = GetRepo();

			var produto = new EntidadeGenericaB(MockValues.NomeGenericoA, MockValues.ValorGenericoA);
			repo.Insert(produto);


			var produtoSelectInsert = GetById(produto);
			Assert.NotNull(produtoSelectInsert);

			produto.Nome = MockValues.NomeGenericoB;
			produto.Valor = MockValues.ValorGenericoB;

			repo.Update(produto);

			var produtoSelectUpdate = GetById(produto);

			Assert.Equal(MockValues.MockUserName, produtoSelectUpdate.LoginAlteracao);
			Assert.True(produtoSelectUpdate.DataHoraAlteracao != null);
			Assert.False(produtoSelectUpdate.Excluido);

			repo.Delete(produto);

			var produtoSelectDelete = GetById(produto);

			Assert.Equal(MockValues.MockUserName, produtoSelectDelete.LoginAlteracao);
			Assert.True(produtoSelectDelete.DataHoraAlteracao > produtoSelectUpdate.DataHoraAlteracao);
			Assert.True(produtoSelectDelete.Excluido);
		}

		[Fact]
		public void DeveInserirVarios()
		{
			var repo = GetRepo();

			var produtos = new List<EntidadeGenericaB>();
			produtos.Add(new EntidadeGenericaB(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
			produtos.Add(new EntidadeGenericaB(MockValues.NomeGenericoA, MockValues.ValorGenericoB));

			repo.Insert(produtos);

			foreach (var produto in produtos)
			{
				Assert.Equal(MockValues.MockUserName, produto.LoginInclusao);
				Assert.True(produto.DataHoraInclusao != null);
				Assert.False(produto.Excluido);
			}
		}

		[Fact]
		public void DeveInserirAtualizarDeletarVarios()
		{
			var repo = GetRepo();

			var produtos = new List<EntidadeGenericaB>();
			produtos.Add(new EntidadeGenericaB(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
			produtos.Add(new EntidadeGenericaB(MockValues.NomeGenericoB, MockValues.ValorGenericoB));

			repo.Insert(produtos);

			produtos[0].Nome = MockValues.NomeGenericoB;
			produtos[0].Valor = MockValues.ValorGenericoB;

			produtos[1].Nome = MockValues.NomeGenericoA;
			produtos[1].Valor = MockValues.ValorGenericoA;

			repo.Update(produtos);

			var produtosUpdates = new List<EntidadeGenericaB>();
			foreach (var produto in produtos)
			{
				var produtoSelectUpdate = GetById(produto);

				Assert.Equal(MockValues.MockUserName, produtoSelectUpdate.LoginAlteracao);
				Assert.True(produtoSelectUpdate.DataHoraAlteracao != null);
				Assert.False(produtoSelectUpdate.Excluido);

				produtosUpdates.Add(produtoSelectUpdate);
			}



			repo.Delete(produtos);

			int i = 0;
			foreach (var produto in produtos)
			{
				var produtoSelectDelete = GetById(produto);

				Assert.Equal(MockValues.MockUserName, produtoSelectDelete.LoginAlteracao);
				Assert.True(produtoSelectDelete.DataHoraAlteracao > produtosUpdates[i].DataHoraAlteracao);
				Assert.True(produtoSelectDelete.Excluido);

				i++;
			}

			
		}
	}
}
