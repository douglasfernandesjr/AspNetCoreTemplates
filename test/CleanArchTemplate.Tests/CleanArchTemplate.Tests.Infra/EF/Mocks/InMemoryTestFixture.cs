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
			contexts = new List<TestDbContext>();
		}

		private string DataBaseName { get; set; }

		private List<TestDbContext> contexts;

		public TestDbContext Context() {
			var db = InMemoryContext(DataBaseName);
			contexts.Add(db);
			return db;
		}

		public void Dispose()
		{
			if (contexts != null) {
				foreach (var context in contexts)
					context?.Dispose();
			}
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
