using Microsoft.EntityFrameworkCore;
using PtgExpressHub.Domain.Entities;

namespace PtgExpressHub.Domain.Data.Configurations;

internal class ApplicationBuildConfiguration
{
    internal static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationBuild>()
            .ToTable("ApplicationBuilds");
        modelBuilder.Entity<ApplicationBuild>()
            .HasKey(b => b.ApplicationBuildId);
        modelBuilder.Entity<ApplicationBuild>()
            .Property(b => b.ApplicationBuildProductionName)
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
            .HasForeignKey(b => b.ApplicationBuildId);
    }
}
