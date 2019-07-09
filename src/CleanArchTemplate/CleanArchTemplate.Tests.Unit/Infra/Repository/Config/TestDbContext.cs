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
		public virtual DbSet<EntidadeGenericaB> EntidadeGenericaB { get; set; }
		public virtual DbSet<EntidadeGenericaC> EntidadeGenericaC { get; set; }

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

			modelBuilder.Entity<EntidadeGenericaB>(entity =>
			{
				entity.ToTable("EntidadeGenericaB");

				entity.Property(e => e.Id);

				entity.Property(e => e.Nome)
					.HasMaxLength(250)
					.IsUnicode(false)
					.IsRequired();

				entity.Property(e => e.Valor)
				.IsRequired();

				entity.Property(e => e.DataHoraInclusao)
				.IsRequired();

				entity.Property(e => e.LoginInclusao)
					.HasMaxLength(250)
					.IsUnicode(false)
					.IsRequired();

				entity.Property(e => e.LoginAlteracao)
					.HasMaxLength(250)
					.IsUnicode(false);

				entity.Property(e => e.DataHoraAlteracao);

				entity.Property(e => e.Excluido).IsRequired();
			});

			modelBuilder.Entity<EntidadeGenericaC>(entity =>
			{
				entity.Property(e => e.Id);

				entity.Property(e => e.Nome)
					.HasMaxLength(250)
					.IsUnicode(false)
					.IsRequired();

				entity.Property(e => e.Valor);

				entity.HasOne(x => x.EntidadeA)
				.WithMany()
				.HasForeignKey(x => x.EntidadeAId)
				.OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}