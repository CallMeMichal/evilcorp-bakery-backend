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
