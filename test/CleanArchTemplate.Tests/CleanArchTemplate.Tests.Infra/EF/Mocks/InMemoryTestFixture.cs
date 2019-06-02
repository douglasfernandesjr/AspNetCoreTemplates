using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchTemplate.Tests.Infra.EF.Mocks
{
	public class InMemoryTestFixture : IDisposable
	{

		public InMemoryTestFixture()
		{
			DataBaseName = Guid.NewGuid().ToString();
		}

		private string DataBaseName { get; set; }

		private TestDbContext context;

		public TestDbContext Context() {
			Dispose();
			context = InMemoryContext(DataBaseName);
			return context;
		}

		public TestDbContext NewContext()
		{
			return InMemoryContext(DataBaseName);
		}

		public void Dispose()
		{
			context?.Dispose();
		}

		private static TestDbContext InMemoryContext(string dbName)
		{
			InMemoryDatabaseRoot InMemoryDatabaseRoot = new InMemoryDatabaseRoot();

			var options = new DbContextOptionsBuilder<TestDbContext>()
					.UseInMemoryDatabase(dbName)
					.EnableSensitiveDataLogging()
					.Options;
			var context = new TestDbContext(options);

			return context;
		}
	}
}
