using Microsoft.EntityFrameworkCore;
using PtgExpressHub.Domain.Data.Configurations;
using PtgExpressHub.Domain.Entities;

public class PtgExpressDataContext : DbContext
{
    public DbSet<ApplicationBuild> ApplicationBuilds { get; set; }
    public DbSet<ApplicationBuildVersion> ApplicationBuildVersions { get; set; }

    public PtgExpressDataContext(DbContextOptions<PtgExpressDataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ApplicationBuildConfiguration.Build(modelBuilder);
        ApplicationBuildVersionConfiguration.Build(modelBuilder);        
    }
}