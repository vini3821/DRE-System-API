using DRESystem.Domain.New;
using Microsoft.EntityFrameworkCore;

namespace DRESystem.Data.New
{
    public class DREContext : DbContext
    {
        public DREContext(DbContextOptions<DREContext> options)
            : base(options) { }

        public DbSet<Region> Regions { get; set; } = null!;
        public DbSet<Sector> Sectors { get; set; } = null!;
        public DbSet<CC> CostCenters { get; set; } = null!;
        public DbSet<Collaborator> Collaborators { get; set; } = null!;
        public DbSet<Entry> Entries { get; set; } = null!;
        public DbSet<Bank> Banks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder
                .Entity<CC>()
                .HasOne(c => c.Region)
                .WithMany(r => r.CostCenters)
                .HasForeignKey(c => c.FKRegion);

            modelBuilder
                .Entity<CC>()
                .HasOne(c => c.Sector)
                .WithMany(s => s.CostCenters)
                .HasForeignKey(c => c.FKSector);

            modelBuilder
                .Entity<Collaborator>()
                .HasOne(c => c.CostCenter)
                .WithMany(cc => cc.Collaborators)
                .HasForeignKey(c => c.FKCC);

            modelBuilder
                .Entity<Entry>()
                .HasOne(e => e.Collaborator)
                .WithMany(c => c.Entries)
                .HasForeignKey(e => e.FKC);
            modelBuilder.Entity<Entry>().Property(e => e.Value).HasPrecision(18, 2);

            modelBuilder
                .Entity<Entry>()
                .HasOne(e => e.Bank)
                .WithMany(b => b.Entries)
                .HasForeignKey(e => e.FKBank)
                .IsRequired(false);
        }
    }
}
