using CleanArchTemplate.Infrastructure.Repository.EF.Base;
using CleanArchTemplate.Tests.Unit.Infra.Repository.Config;
using CleanArchTemplate.Tests.Unit.Infra.Repository.Config.Entities;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchTemplate.Tests.Unit.Infra.Repository
{
	public class EFSearchRepositoryAsyncTests : IClassFixture<InMemoryTestFixtureSQLLite>
	{
		private readonly InMemoryTestFixtureSQLLite _fixture;

		public EFSearchRepositoryAsyncTests(InMemoryTestFixtureSQLLite fixture)
		{
			_fixture = fixture;
			_fixture.Clean();
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

		private EFRepository<EntidadeGenericaA> GetRepo()
		{
			return new EFRepository<EntidadeGenericaA>(_fixture.Context());
		}

		//http://www.anarsolutions.com/automated-unit-testing-tools-comparison/
		//Given_Preconditions_When_StateUnderTest_Then_ExpectedBehavior
		//When_Deposit_Is_Made_Should_Increase_Balance


		#region ExecuteSearch
		[Fact]
		public async Task Given_PA11B11_When_ListAll_Then_ListAll()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var list = await repo.NewSearch().All().SearchAsync();

			Assert.Equal(22, list.Count());
		}

		#endregion ExecuteSearch

		#region Count
		[Fact]
		public async Task Given_PA11B11_When_Count_Then_Return22()
		{
			PopulateA11B11();
			var repo = GetRepo();

			int count = await repo.NewSearch().All().CountAsync();

			Assert.Equal(22, count);
		}

		[Fact]
		public async Task Given_PA11B11_When_WhereCount_Then_Return11()
		{
			PopulateA11B11();
			var repo = GetRepo();

			int count = await repo.NewSearch().Where(x => x.Valor == MockValues.ValorGenericoB).CountAsync();

			Assert.Equal(11, count);
		}
		#endregion Count

		#region SmartPageSearch
		[Fact]
		public async Task Given_PA11B11_When_SmartPageSearch_0_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = await repo.NewSearch().All().SmartPagedSearchAsync(0,10);

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
		public async Task Given_PA11B11_When_SmartPageSearch_1_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = await repo.NewSearch().All().SmartPagedSearchAsync(1, 10);

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
		public async Task Given_PA11B11_When_SmartPageSearch_2_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = await repo.NewSearch().All().SmartPagedSearchAsync(2, 10);

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
		public async Task Given_PA11B11_When_PageSearchWithCount_0_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = await repo.NewSearch().All().PagedSearchAsync(0, 10);

			Assert.NotNull(page);
			Assert.Equal(0, page.PageIndex);
			Assert.Equal(10, page.PageSize);
			Assert.False(page.LastPage);
			Assert.Equal(3,page.TotalPages);
			Assert.Equal(22,page.TotalItens);
			Assert.NotNull(page.Data);
			Assert.Equal(10, page.Data.Count());
		}

		[Fact]
		public async Task Given_PA11B11_When_PageSearchWithCount_1_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = await repo.NewSearch().All().PagedSearchAsync(1, 10);

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
		public async Task Given_PA11B11_When_PageSearchWithCount_2_10_Then_Page()
		{
			PopulateA11B11();
			var repo = GetRepo();

			var page = await repo.NewSearch().All().PagedSearchAsync(2, 10);

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