using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace CleanArchTemplate.Tests.Unit.Infra.Repository.Config
{
	public class InMemoryTestFixtureSQLLite : IDisposable
	{
		private DbConnection _connection;

		private DbContextOptions<TestDbContext> CreateOptions()
		{
			return new DbContextOptionsBuilder<TestDbContext>()
				.UseSqlite(_connection).Options;
		}
		public void Clean() {

			if (_connection != null) {
				_connection.Dispose();
				_connection = null;
			}
		}

		public TestDbContext Context()
		{
			if (_connection == null)
			{
				_connection = new SqliteConnection("DataSource=:memory:");
				_connection.Open();

				var options = CreateOptions();
				using (var context = new TestDbContext(options))
				{
					context.Database.EnsureCreated();
				}
			}

			return new TestDbContext(CreateOptions());
		}

		public void Dispose()
		{
			if (_connection != null)
			{
				_connection.Dispose();
				_connection = null;
			}
		}
	}
}
