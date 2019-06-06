using CleanArchTemplate.Tests.Unit.Infra.Repository.Config.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchTemplate.Tests.Unit.Infra.Repository.Config
{
	public class TestDbContext : DbContext
	{
		public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
		{
		}

		public virtual DbSet<EntidadeGenericaA> EntidadeGenericaA { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EntidadeGenericaA>(entity =>
			{
				entity.ToTable("EntidadeGenericaA");

				entity.Property(e => e.Id);

				entity.Property(e => e.Nome)
					.HasMaxLength(250)
					.IsUnicode(false)
					.IsRequired();

				entity.Property(e => e.Valor)
				.IsRequired();
			});
		}
	}
}
