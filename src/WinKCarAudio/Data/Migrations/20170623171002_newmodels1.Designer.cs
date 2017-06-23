using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WinKCarAudio.Data;

namespace WinKCarAudio.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170623171002_newmodels1")]
    partial class newmodels1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WinKCarAudio.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WinKCarAudio.Models.AssetViewModels.Asset", b =>
                {
                    b.Property<int>("assetID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ImageGalleryId");

                    b.Property<int>("MakeId");

                    b.Property<DateTime>("addDate");

                    b.Property<string>("description");

                    b.Property<bool>("featuredItem");

                    b.Property<string>("name")
                        .IsRequired();

                    b.Property<decimal>("price");

                    b.HasKey("assetID");

                    b.HasIndex("ImageGalleryId")
                        .IsUnique();

                    b.HasIndex("MakeId");

                    b.ToTable("Asset");
                });

            modelBuilder.Entity("WinKCarAudio.Models.AssetViewModels.AssetCategory", b =>
                {
                    b.Property<int>("AssetId");

                    b.Property<int>("CategoryId");

                    b.HasKey("AssetId", "CategoryId");

                    b.HasIndex("AssetId");

                    b.HasIndex("CategoryId");

                    b.ToTable("AssetCategory");
                });

            modelBuilder.Entity("WinKCarAudio.Models.AssetViewModels.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryName");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("WinKCarAudio.Models.AssetViewModels.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileLink")
                        .IsRequired();

                    b.Property<int?>("ImageGalleryId")
                        .IsRequired();

                    b.Property<Guid>("ImageGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.Property<bool>("isMain");

                    b.HasKey("ImageId");

                    b.HasAlternateKey("ImageGuid");

                    b.HasIndex("ImageGalleryId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("WinKCarAudio.Models.AssetViewModels.ImageGallery", b =>
                {
                    b.Property<int>("ImageGalleryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Title");

                    b.HasKey("ImageGalleryId");

                    b.ToTable("ImageGallery");
                });

            modelBuilder.Entity("WinKCarAudio.Models.AssetViewModels.Make", b =>
                {
                    b.Property<int>("MakeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("MakeId");

                    b.ToTable("Make");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WinKCarAudio.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WinKCarAudio.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WinKCarAudio.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WinKCarAudio.Models.AssetViewModels.Asset", b =>
                {
                    b.HasOne("WinKCarAudio.Models.AssetViewModels.ImageGallery", "ImageGallery")
                        .WithOne("Asset")
                        .HasForeignKey("WinKCarAudio.Models.AssetViewModels.Asset", "ImageGalleryId");

                    b.HasOne("WinKCarAudio.Models.AssetViewModels.Make", "Make")
                        .WithMany("Assets")
                        .HasForeignKey("MakeId");
                });

            modelBuilder.Entity("WinKCarAudio.Models.AssetViewModels.AssetCategory", b =>
                {
                    b.HasOne("WinKCarAudio.Models.AssetViewModels.Asset", "Asset")
                        .WithMany("AssetCategories")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WinKCarAudio.Models.AssetViewModels.Category", "Category")
                        .WithMany("AssetCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WinKCarAudio.Models.AssetViewModels.Image", b =>
                {
                    b.HasOne("WinKCarAudio.Models.AssetViewModels.ImageGallery", "ImageGallery")
                        .WithMany("Images")
                        .HasForeignKey("ImageGalleryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
