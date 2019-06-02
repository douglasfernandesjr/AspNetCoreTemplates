using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchTemplate.Tests.Infra.EF
{
	public class InMemoryTestFixture : IDisposable
	{
		public TestDbContext Context => InMemoryContext();

		public void Dispose()
		{
			Context?.Dispose();
		}

		private static TestDbContext InMemoryContext()
		{
			var options = new DbContextOptionsBuilder<TestDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.EnableSensitiveDataLogging()
				.Options;
			var context = new TestDbContext(options);

			return context;
		}
	}
}
