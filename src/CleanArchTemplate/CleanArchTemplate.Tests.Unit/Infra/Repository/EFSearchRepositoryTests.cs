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
	public class EFSearchRepositoryTests : IClassFixture<InMemoryTestFixtureSQLLite>
	{
		private readonly InMemoryTestFixtureSQLLite _fixture;

		public EFSearchRepositoryTests(InMemoryTestFixtureSQLLite fixture)
		{
			_fixture = fixture;

			Populate();
			//Executa antes de cada teste, limpar o banco aqui.
		}

	
		private void Populate()
		{

			var db = _fixture.Context();

			for (int i = 0; i < 11; i++) {
				db.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
				db.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoB));
			}
			db.SaveChanges();
		}

		
		private EFRepository<EntidadeGenericaA> GetRepo()
		{
			return new EFRepository<EntidadeGenericaA>(_fixture.Context());
		}

		#region Testes Pesquisa Básica

		//http://www.anarsolutions.com/automated-unit-testing-tools-comparison/
		//Given_Preconditions_When_StateUnderTest_Then_ExpectedBehavior
		//When_Deposit_Is_Made_Should_Increase_Balance

		[Fact]
		public void When_ListAll_Should_ListAll()
		{
			var repo = GetRepo();

			var list = repo.NewSearch().All().ExecuteSearch();

			Assert.Equal(22,list.Count());
		}
		[Fact]
		public void When_FilterByname_Should_FilterByname()
		{
			var repo = GetRepo();

			var list = repo.NewSearch().Where(x=>x.Nome == MockValues.NomeGenericoB).ExecuteSearch();

			Assert.Equal(11, list.Count());
		}


		[Fact]
		public void When_ListAll_Should_ListAll2()
		{
			var repo = GetRepo();

			var list = repo.NewSearch().All().ExecuteSearch();

			Assert.Equal(22, list.Count());
		}
		#endregion Testes Pesquisa Básica

	}
}
