﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace PtgExpressHub.Domain.Data.Migrations
{
    [DbContext(typeof(PtgExpressDataContext))]
    partial class PtgExpressDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PtgExpressHub.Domain.Entities.ApplicationBuild", b =>
                {
                    b.Property<Guid>("ApplicationBuildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApplicationBuildProductionName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ApplicationBuildUserName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ApplicationRepositoryUrl")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("ApplicationBuildId");

                    b.ToTable("ApplicationBuilds", (string)null);
                });

            modelBuilder.Entity("PtgExpressHub.Domain.Entities.ApplicationBuildVersion", b =>
                {
                    b.Property<Guid>("ApplicationVersionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationBuildId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BlobUrl")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ChangeLog")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("ApplicationVersionId");

                    b.HasIndex("ApplicationBuildId");

                    b.ToTable("ApplicationBuildVersions", (string)null);
                });

            modelBuilder.Entity("PtgExpressHub.Domain.Entities.ApplicationBuildVersion", b =>
                {
                    b.HasOne("PtgExpressHub.Domain.Entities.ApplicationBuild", "ApplicationBuild")
                        .WithMany("ApplicationBuildVersions")
                        .HasForeignKey("ApplicationBuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationBuild");
                });

            modelBuilder.Entity("PtgExpressHub.Domain.Entities.ApplicationBuild", b =>
                {
                    b.Navigation("ApplicationBuildVersions");
                });
#pragma warning restore 612, 618
        }
    }
}
