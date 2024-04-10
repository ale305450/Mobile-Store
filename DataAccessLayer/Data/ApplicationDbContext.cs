using DataAccessLayer.Entites;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        //public DbSet<CompanyViewModel> CompanyViewModel { get; set; }
        public DbSet<CustomerReview> CustomerReview { get; set; }
        //public DbSet<CustomerReviewViewModel> CustomerReviewViewModel { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<ProductViewModel> ProductsViewModel { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    options => options.MigrationsAssembly("DataAccessLayer"));
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //RelationShip
            builder.Entity<Orders>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Orders>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(m => m.productId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Company>()
                .HasMany<Product>()
                .WithOne()
                .HasForeignKey(m => m.companyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Category>()
                .HasMany<Product>()
                .WithOne()
                .HasForeignKey(c => c.categoryId)
                .OnDelete(DeleteBehavior.Cascade);

            
            //Company
            builder.Entity<Company>().Property(c => c.name).IsRequired();
            builder.Entity<Company>().Property(c => c.description).HasMaxLength(int.MaxValue);

            //CustomerReview
            builder.Entity<CustomerReview>().Property(c => c.name).IsRequired();
            builder.Entity<CustomerReview>().Property(c => c.email).IsRequired();
            builder.Entity<CustomerReview>().Property(c => c.message).IsRequired();
            builder.Entity<CustomerReview>().Property(c => c.message).HasMaxLength(int.MaxValue);

            //Product
            builder.Entity<Product>().Property(c => c.name).IsRequired();
            builder.Entity<Product>().Property(c => c.price).IsRequired();
            builder.Entity<Product>().Property(c => c.image).IsRequired();
            builder.Entity<Product>().Property(c => c.companyId).IsRequired();
            builder.Entity<Product>().Property(c => c.categoryId).IsRequired();
            builder.Entity<Product>().Property(c => c.description).HasMaxLength(int.MaxValue);

            //Category
            builder.Entity<Category>().HasData(
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Mobile Products"
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Smart Watches"
                }
            );
        }
    }
}
