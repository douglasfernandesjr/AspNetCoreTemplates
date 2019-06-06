using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CleanArchTemplate.Tests.Unit.Infra.Repository.Config
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

		public TestDbContext Context()
		{
			var db = InMemoryContext(DataBaseName);
			contexts.Add(db);
			return db;
		}

		public void Dispose()
		{
			if (contexts != null)
			{
				foreach (var context in contexts)
					context?.Dispose();
			}
		}

		private static TestDbContext InMemoryContext(string dbName)
		{
			//https://www.meziantou.net/2017/09/11/testing-ef-core-in-memory-using-sqlite
			var options = new DbContextOptionsBuilder<TestDbContext>()
					//.UseSqlite("DataSource=:memory:")
					.UseInMemoryDatabase(dbName)
					.EnableSensitiveDataLogging()
					.Options;
			var context = new TestDbContext(options);

			return context;
		}
	}
}
