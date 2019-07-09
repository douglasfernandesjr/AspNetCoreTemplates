using CleanArchTemplate.Infrastructure.Repository.EF.Base;
using CleanArchTemplate.Tests.Unit.Infra.Repository.Config;
using CleanArchTemplate.Tests.Unit.Infra.Repository.Config.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CleanArchTemplate.Tests.Unit.Infra.Repository
{
	public class EFSearchRepositoryTests : IClassFixture<InMemoryTestFixtureSQLLite>
	{
		private readonly InMemoryTestFixtureSQLLite _fixture;

		public EFSearchRepositoryTests(InMemoryTestFixtureSQLLite fixture)
		{
			_fixture = fixture;

			_fixture.Clean();
			//Executa antes de cada teste, limpar o banco aqui.
		}

		private void PopulateA11B11()
		{
			var db = _fixture.Context();

			for (int i = 0; i < 11; i++)
			{
				db.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
				db.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoB));
			}
			db.SaveChanges();
		}

		private void PopulateA11B11C11()
		{
			var db = _fixture.Context();

			var lA = new List<EntidadeGenericaA>();

			for (int i = 0; i < 11; i++)
			{
				lA.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
				db.Add(lA[i]);
				db.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoB));
			}
			db.SaveChanges();

			for (int i = 0; i < 11; i++)
			{
				var c = new EntidadeGenericaC(MockValues.NomeGenericoA, MockValues.ValorGenericoA)
				{
					EntidadeAId = lA[0].Id
				};
				db.Add(c);
			}
			db.SaveChanges();
		}

		private EFRepository<EntidadeGenericaA> GetRepo()
		{
			return new EFRepository<EntidadeGenericaA>(_fixture.Context());
		}

		private EFRepository<EntidadeGenericaC> GetRepoC()
		{
			return new EFRepository<EntidadeGenericaC>(_fixture.Context());
		}

		//http://www.anarsolutions.com/automated-unit-testing-tools-comparison/
		//Given_Preconditions_When_StateUnderTest_Then_ExpectedBehavior
		//When_Deposit_Is_Made_Should_Increase_Balance
		#region Entity
		[Fact]
		public void Given_PA11B11C11_When_NotAddEntityCListAll_Then_ListAll()
		{
			PopulateA11B11C11();

			var list = GetRepoC().NewSearch().All().Search();

			Assert.Equal(11, list.Count());
			Assert.Null(list.ElementAt(0).EntidadeA);
		}
		[Fact]
		public void Given_PA11B11C11_When_AddEntityCListAll_Then_ListAll()
		{
			PopulateA11B11C11();

			var list = GetRepoC().NewSearch().All().IncludeEntity(x => x.EntidadeA).Search();

			Assert.Equal(11, list.Count());
			Assert.NotNull(list.ElementAt(0).EntidadeA);
		}
		#endregion Entity

		#region ExecuteSearch

		[Fact]
		public void Given_PA11B11_When_ListAll_Then_ListAll()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var list = repo.NewSearch().All().Search();

			Assert.Equal(22, list.Count());
		}

		[Fact]
		public void Given_PA11B11_When_FilterByname_Then_FilterByname()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var list = repo.NewSearch().Where(x => x.Nome == MockValues.NomeGenericoB).Search();

			Assert.Equal(11, list.Count());
		}

		[Fact]
		public void Given_PA11B11_When_ListAllOrderByAsc_Then_ListAllOrderByAsc()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var list = repo.NewSearch().All().OrderByAsc(x => x.Nome).Search();

			Assert.Equal(22, list.Count());
			Assert.Equal(list.ElementAt(0).Nome, MockValues.NomeGenericoB);
			Assert.Equal(list.ElementAt(10).Nome, MockValues.NomeGenericoB);
			Assert.Equal(list.ElementAt(11).Nome, MockValues.NomeGenericoA);
		}

		[Fact]
		public void Given_PA11B11_When_ListAllOrderByDesc_Then_ListAllOrderByDesc()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var list = repo.NewSearch().All().OrderByDesc(x => x.Nome).Search();

			Assert.Equal(22, list.Count());
			Assert.Equal(list.ElementAt(0).Nome, MockValues.NomeGenericoA);
			Assert.Equal(list.ElementAt(10).Nome, MockValues.NomeGenericoA);
			Assert.Equal(list.ElementAt(11).Nome, MockValues.NomeGenericoB);
		}
		#endregion ExecuteSearch

		#region Count
		[Fact]
		public void Given_PA11B11_When_Count_Then_Return22()
		{
			PopulateA11B11();
			var repo = GetRepo();

			int count = repo.NewSearch().All().Count();

			Assert.Equal(22, count);
		}

		[Fact]
		public void Given_PA11B11_When_WhereCount_Then_Return11()
		{
			PopulateA11B11();
			var repo = GetRepo();

			int count = repo.NewSearch().Where(x => x.Valor == MockValues.ValorGenericoB).Count();

			Assert.Equal(11, count);
		}
		#endregion Count

		#region SmartPageSearch
		[Fact]
		public void Given_PA11B11_When_SmartPageSearch_0_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = repo.NewSearch().All().SmartPagedSearch(0, 10);

			Assert.NotNull(page);
			Assert.Equal(0, page.PageIndex);
			Assert.Equal(10, page.PageSize);
			Assert.False(page.LastPage);
			Assert.Null(page.TotalPages);
			Assert.Null(page.TotalItens);
			Assert.NotNull(page.Data);
			Assert.Equal(10, page.Data.Count());
		}

		[Fact]
		public void Given_PA11B11_When_SmartPageSearch_1_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = repo.NewSearch().All().SmartPagedSearch(1, 10);

			Assert.NotNull(page);
			Assert.Equal(1, page.PageIndex);
			Assert.Equal(10, page.PageSize);
			Assert.False(page.LastPage);
			Assert.Null(page.TotalPages);
			Assert.Null(page.TotalItens);
			Assert.NotNull(page.Data);
			Assert.Equal(10, page.Data.Count());
		}

		[Fact]
		public void Given_PA11B11_When_SmartPageSearch_2_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = repo.NewSearch().All().SmartPagedSearch(2, 10);

			Assert.NotNull(page);
			Assert.Equal(2, page.PageIndex);
			Assert.Equal(10, page.PageSize);
			Assert.True(page.LastPage);
			Assert.Null(page.TotalPages);
			Assert.Null(page.TotalItens);
			Assert.NotNull(page.Data);
			Assert.Equal(2, page.Data.Count());
		}
		#endregion SmartPageSearch

		#region PageSearchWithCount
		[Fact]
		public void Given_PA11B11_When_PageSearchWithCount_0_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = repo.NewSearch().All().PagedSearch(0, 10);

			Assert.NotNull(page);
			Assert.Equal(0, page.PageIndex);
			Assert.Equal(10, page.PageSize);
			Assert.False(page.LastPage);
			Assert.Equal(3, page.TotalPages);
			Assert.Equal(22, page.TotalItens);
			Assert.NotNull(page.Data);
			Assert.Equal(10, page.Data.Count());
		}

		[Fact]
		public void Given_PA11B11_When_PageSearchWithCount_1_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = repo.NewSearch().All().PagedSearch(1, 10);

			Assert.NotNull(page);
			Assert.Equal(1, page.PageIndex);
			Assert.Equal(10, page.PageSize);
			Assert.False(page.LastPage);
			Assert.Equal(3, page.TotalPages);
			Assert.Equal(22, page.TotalItens);
			Assert.NotNull(page.Data);
			Assert.Equal(10, page.Data.Count());
		}

		[Fact]
		public void Given_PA11B11_When_PageSearchWithCount_2_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = repo.NewSearch().All().PagedSearch(2, 10);

			Assert.NotNull(page);
			Assert.Equal(2, page.PageIndex);
			Assert.Equal(10, page.PageSize);
			Assert.True(page.LastPage);
			Assert.Equal(3, page.TotalPages);
			Assert.Equal(22, page.TotalItens);
			Assert.NotNull(page.Data);
			Assert.Equal(2, page.Data.Count());
		}

		#endregion PageSearchWithCount
	}
}