using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public partial class ETDatabaseContext : DbContext
    {
        public ETDatabaseContext(DbContextOptions<ETDatabaseContext> options) : base(options)
        {

        }

        public virtual DbSet<Adverts> Adverts { get; set; }
        public virtual DbSet<AdvertsCategories> AdvertsCategories { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(@"Server=PIPCZAR-KOMP;Database=ETDatabase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adverts>(entity =>
            {
                entity.HasKey(e => e.AdvertId);

                entity.Property(e => e.AdvertId).HasColumnName("AdvertID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.DueDate).HasColumnType("datetime2(0)");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.StartDate).HasColumnType("datetime2(0)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Adverts)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Adverts_Locations");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Adverts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Adverts_Users");
            });

            modelBuilder.Entity<AdvertsCategories>(entity =>
            {
                entity.HasKey(e => new { e.AdvertId, e.CategoryId });

                entity.Property(e => e.AdvertId).HasColumnName("AdvertID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.HasOne(d => d.Advert)
                    .WithMany(p => p.AdvertsCategories)
                    .HasForeignKey(d => d.AdvertId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AtdvertsCategories_Adverts");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.AdvertsCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdvertsCategories_Categories");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.County).HasMaxLength(50);

                entity.Property(e => e.Region)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.Nickname)
                    .HasName("AK_Users_Nickname")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.Property(e => e.PhoneNumber).HasMaxLength(11);
            });
        }
    }
}
