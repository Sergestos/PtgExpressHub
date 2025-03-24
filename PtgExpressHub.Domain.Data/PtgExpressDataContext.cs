using Microsoft.EntityFrameworkCore;
using PtgExpressHub.Domain.Entities;

public class PtgExpressDataContext : DbContext
{
    public DbSet<ApplicationBuild> ApplicationBuilds { get; set; }

    public DbSet<ApplicationBuildVersion> ApplicationBuildVersions { get; set; }

    public PtgExpressDataContext(DbContextOptions<PtgExpressDataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationBuild>().ToTable("ApplicationBuilds");
        modelBuilder.Entity<ApplicationBuild>()
            .HasKey(b => b.ApplicationBuildId);
        modelBuilder.Entity<ApplicationBuild>()
            .Property(b => b.ApplicationBuildProductName)
            .HasMaxLength(128)
            .IsRequired();
        modelBuilder.Entity<ApplicationBuild>()
            .Property(b => b.ApplicationBuildUserName)
            .HasMaxLength(128)
            .IsRequired();
        modelBuilder.Entity<ApplicationBuild>()
            .Property(b => b.ApplicationRepositoryUrl)
            .HasMaxLength(128)
            .IsRequired();
        modelBuilder.Entity<ApplicationBuild>()
            .HasMany(b => b.ApplicationBuildVersions)
            .WithOne(p => p.ApplicationBuild)
            .HasForeignKey(b => b.ApplicationId);

        modelBuilder.Entity<ApplicationBuildVersion>().ToTable("ApplicationBuildVersions");
        modelBuilder.Entity<ApplicationBuildVersion>()
            .HasKey(b => b.ApplicationVersionId);
        modelBuilder.Entity<ApplicationBuildVersion>()
            .Property(b => b.ChangeLog)
            .HasMaxLength(512)
            .IsRequired();
        modelBuilder.Entity<ApplicationBuildVersion>()
            .Property(b => b.BlobUrl)
            .HasMaxLength(128)
            .IsRequired();
        modelBuilder.Entity<ApplicationBuildVersion>()
            .Property(b => b.Version)
            .HasMaxLength(32)
            .IsRequired();
        modelBuilder.Entity<ApplicationBuildVersion>()
            .Property(b => b.UploadDate)
            .IsRequired();
        modelBuilder.Entity<ApplicationBuildVersion>()
            .HasOne(b => b.ApplicationBuild)
            .WithMany(p => p.ApplicationBuildVersions)
            .HasForeignKey(b => b.ApplicationId)
            .IsRequired();
    }
}