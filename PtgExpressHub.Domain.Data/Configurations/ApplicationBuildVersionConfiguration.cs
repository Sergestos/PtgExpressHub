using Microsoft.EntityFrameworkCore;
using PtgExpressHub.Domain.Entities;

namespace PtgExpressHub.Domain.Data.Configurations;

internal class ApplicationBuildVersionConfiguration
{
    internal static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationBuildVersion>()
            .ToTable("ApplicationBuildVersions");
        modelBuilder.Entity<ApplicationBuildVersion>()
            .HasKey(b => b.ApplicationVersionId);
        modelBuilder.Entity<ApplicationBuildVersion>()
            .Property(b => b.ChangeLog)
            .HasMaxLength(2048)
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
            .HasForeignKey(b => b.ApplicationBuildId)
            .IsRequired();
    }
}
