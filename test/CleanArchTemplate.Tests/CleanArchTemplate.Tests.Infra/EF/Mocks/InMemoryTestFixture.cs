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
			Context = InMemoryContext(DataBaseName);
		}

		private string DataBaseName { get; set; }

		public TestDbContext Context { get; set; }

		public TestDbContext NewContext()
		{
			return InMemoryContext(DataBaseName);
		}

		public void Dispose()
		{
			Context?.Dispose();
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
