using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace SamplePeteService.Models
{
    public partial class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public virtual DbSet<JncProjectEmployee> JncProjectEmployees { get; set; }
        public virtual DbSet<TblEmployeeInfo> TblEmployeeInfos { get; set; }
        public virtual DbSet<TblProject> TblProjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = null;
                bool useInMemoryDatabase = true;

                try
                {
                    configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                        .AddJsonFile("appsettings.json", false)
                        .Build();

                    useInMemoryDatabase = configuration.GetSection("PeteSampleAppSettings:UseInMemoryDatabase").Value != null && bool.Parse(configuration.GetSection("PeteSampleAppSettings:UseInMemoryDatabase").Value);
                }
                catch (Exception)
                {
                    // appsettings.json file is missing
                    // default to using in-memory-database
                    useInMemoryDatabase = true;
                }

                if (useInMemoryDatabase)
                {
                    optionsBuilder.UseInMemoryDatabase("SamplePeteAppDB")
                        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)); // don't raise the error warning us that the in-memory-database doesn't support tranactions
                }
                else
                {
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("PETESAMPLEAPPCONNECTION"));
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<JncProjectEmployee>(entity =>
            {
                entity.HasKey(e => new { e.FkProjectID, e.FkEmployeeID });

                entity.ToTable("jncProjectEmployees");

                entity.Property(e => e.FkProjectID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("fkProjectID");

                entity.Property(e => e.FkEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("fkEmployeeID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.FkEmployee)
                    .WithMany(p => p.JncProjectEmployees)
                    .HasForeignKey(d => d.FkEmployeeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_jncProjectEmployees_tblEmployeeInfo1");

                entity.HasOne(d => d.FkProject)
                    .WithMany(p => p.JncProjectEmployees)
                    .HasForeignKey(d => d.FkProjectID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_jncProjectEmployees_tblEmployeeInfo");
            });

            modelBuilder.Entity<TblEmployeeInfo>(entity =>
            {
                entity.HasKey(e => e.EmployeeID);

                entity.ToTable("tblEmployeeInfo");

                entity.Property(e => e.EmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.DateHired).HasColumnType("datetime");

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.PositionTitle)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblProject>(entity =>
            {
                entity.HasKey(e => e.ProjectID);

                entity.ToTable("tblProjects");

                entity.Property(e => e.ProjectID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ProjectID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ProjectName).IsRequired();

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
