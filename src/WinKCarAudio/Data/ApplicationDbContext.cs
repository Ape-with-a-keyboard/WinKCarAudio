using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WinKCarAudio.Models;
using WinKCarAudio.Models.AssetViewModels;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WinKCarAudio.Data
{
        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext() { }

            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
                // Customize the ASP.NET Identity model and override the defaults if needed.
                // For example, you can rename the ASP.NET Identity table names and more.
                // Add your customizations after calling base.OnModelCreating(builder);

                /**
                 * Configure the Many-to-Many Asset and Category relationship via Fluent API
                 */
                builder.Entity<AssetCategory>()
                    .HasKey(ac => new { ac.AssetId, ac.CategoryId });

                builder.Entity<AssetCategory>()
                    .HasOne(ac => ac.Asset)
                    .WithMany(a => a.AssetCategories)
                    .HasForeignKey(ac => ac.AssetId);

                builder.Entity<AssetCategory>()
                    .HasOne(ac => ac.Category)
                    .WithMany(a => a.AssetCategories)
                    .HasForeignKey(ac => ac.CategoryId);

                /**
                 * Configure One-to-Many relationship with a self-referincing table
                 */
                builder.Entity<Category>()
                   .HasKey(c => c.CategoryId);

                /**
                 * Configure one-to-many relationship between Make and Asset
                 */
                builder.Entity<Make>()
                   .HasMany<Asset>(g => g.Assets)
                   .WithOne(g => g.Make)
                   .HasForeignKey(g => g.MakeId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

                /**
                 * Configure one-to-many relationship between Image and ImageGallery
                 */
                builder.Entity<ImageGallery>()
                   .HasMany<Image>(g => g.Images)
                   .WithOne(g => g.ImageGallery)
                   .HasForeignKey(g => g.ImageGalleryId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

                /**
                 * Configure one-to-one relationship between Asset and ImageGallery
                 */
                /*
                builder.Entity<ImageGallery>()
                   .HasOne(a => a.Asset)
                   .WithOne(i => i.ImageGallery)
                   .HasForeignKey<Asset>(g => g.ImageGalleryId)
                   .IsRequired(false);
                   */

                /**
                 * Configure alternate key for image
                 */
                builder.Entity<Image>()
                    .HasAlternateKey(c => c.ImageGuid);

            }

            public DbSet<Asset> Asset { get; set; }
            public DbSet<Category> Category { get; set; }
            public DbSet<AssetCategory> AssetCategory { get; set; }
            public DbSet<Make> Make { get; set; }
            public DbSet<Image> Image { get; set; }
            public DbSet<ImageGallery> ImageGallery { get; set; }

            public DbSet<ApplicationUser> User { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;");
                }
            }
        }
}
