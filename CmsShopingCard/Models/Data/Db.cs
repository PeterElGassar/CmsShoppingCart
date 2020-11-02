using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
namespace CmsShopingCard.Models.Data
{
    public class Db : DbContext
    {
        public DbSet<Page> Pages { get; set; }
        public DbSet<Sidebar> Sidebar { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }


        public DbSet<Brand> Brands { get; set; }
        public DbSet<CategoryBrand> CategoryBrands { get; set; }

        public DbSet<SliderGallery> SliderGallerys { get; set; }

        public DbSet<WishList> WishLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Turn Off Cascade Delete
            //modelBuilder.Entity<Attendance>()
            //    .HasRequired(a => a.Gig)
            //    .WithMany(g => g.Attendances)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<WishList>().HasRequired(w => w.Product)
                .WithMany(p => p.WishLists)
                .WillCascadeOnDelete(false);



            //modelBuilder.Entity<WishList>().HasRequired(u=> u.User)
            //    .WithOptional(u=> u.)
        }



    }
}