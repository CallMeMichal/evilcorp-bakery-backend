using EvilCorpBakery.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Data
{
    public class EvliCorpBakeryAppDbContext : DbContext
    {

    public EvliCorpBakeryAppDbContext(DbContextOptions<EvliCorpBakeryAppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
