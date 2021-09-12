using System;
using System.Collections.Generic;
using System.Text;
using DynamicMenuProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DynamicMenuProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MenuItems> MenuItems { get; set; }
        public DbSet<MenuPermissions> MenuPermissions { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<CMSItems> CMSItems { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<FeedbackForm> FeedbackForm { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<RelatedValues> RelatedValues { get; set; }
        public DbSet<FeedbackResult> FeedbackResult { get; set; }
        public DbSet<RelatedProcedure> RelatedProcedure { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<ProductNew> ProductNew { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<CartNew> CartNew { get; set; }
        public DbSet<OrderNew> OrderNew { get; set; }
        public DbSet<OrderDetailsNew> OrderDetailsNew { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<PayerInfo> PayerInfo { get; set; }
    }
}
