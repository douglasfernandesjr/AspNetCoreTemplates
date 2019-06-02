using CleanArchTemplate.Tests.Infra.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchTemplate.Tests.Infra.EF
{
	public class TestDbContext : DbContext
	{
		public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
		{
		}

		public virtual DbSet<Produto> Produto { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Produto>(entity =>
			{
				entity.ToTable("Produto");

				entity.Property(e => e.Id);

				entity.Property(e => e.Nome)
					.HasColumnType("varchar(250)")
					.IsRequired();

				entity.Property(e => e.Valor)
				.IsRequired();
			});
		}
	}
}