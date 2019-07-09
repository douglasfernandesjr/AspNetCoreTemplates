using CleanArchTemplate.Tests.Unit.Infra.Repository.Config.Entities;
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

		#region Populate

		public void PopulateA11B11()
		{
			var db = Context();

			for (int i = 0; i < 11; i++)
			{
				db.Add(new EntidadeGenericaA(MockValues.NomeGenericoA, MockValues.ValorGenericoA));
				db.Add(new EntidadeGenericaA(MockValues.NomeGenericoB, MockValues.ValorGenericoB));
			}
			db.SaveChanges();
		}

		public void PopulateA11B11C11()
		{
			var db = Context();

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

		#endregion
	}
}
