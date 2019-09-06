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
			_fixture.Clean();
		}

		private EFRepositoryAudit<EntidadeGenericaB> GetRepo()
		{
			return new EFRepositoryAudit<EntidadeGenericaB>(MockValues.GetMockUser(), _fixture.Context());
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

		//http://www.anarsolutions.com/automated-unit-testing-tools-comparison/
		//Given_Preconditions_When_StateUnderTest_Then_ExpectedBehavior
		//When_Deposit_Is_Made_Should_Increase_Balance
		[Fact]
		public void When_Insert_Should_InsertWithLogData()
		{
			var repo = GetRepo();

			var produto = new EntidadeGenericaB(MockValues.NomeGenericoA, MockValues.ValorGenericoA);

			repo.Insert(produto);

			Assert.Equal(MockValues.MockUserName, produto.LoginInclusao);
			Assert.True(produto.DataHoraInclusao != null);
			Assert.False(produto.Excluido);
		}


		[Fact]
		public void When_InsertList_Should_InsertWithLogData()
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
		public void When_Update_Should_UpdateWithLogData()
		{
			int i = _fixture.PopulateB1();
			var repo = GetRepo();

			var produto = GetById(i);

			produto.Nome = MockValues.NomeGenericoB;
			produto.Valor = MockValues.ValorGenericoB;

			repo.Update(produto);

			var produtoSelectUpdate = GetById(produto);

			Assert.Equal(MockValues.MockUserName, produtoSelectUpdate.LoginAlteracao);
			Assert.NotNull(produtoSelectUpdate.DataHoraAlteracao);
			Assert.False(produtoSelectUpdate.Excluido);


		}

		[Fact]
		public void When_Delete_Should_DeleteWithLogData()
		{
			int i = _fixture.PopulateB1();
			var repo = GetRepo();

			var produto = GetById(i);

			produto.Nome = MockValues.NomeGenericoB;
			produto.Valor = MockValues.ValorGenericoB;

			repo.Delete(produto);

			var produtoSelectDelete = GetById(produto);

			Assert.Equal(MockValues.MockUserName, produtoSelectDelete.LoginAlteracao);
			Assert.NotNull(produtoSelectDelete.DataHoraAlteracao);
			Assert.True(produtoSelectDelete.Excluido);
		}
		//TODO separar
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
