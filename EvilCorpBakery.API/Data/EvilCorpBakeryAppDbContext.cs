using EvilCorpBakery.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Data
{
    public class EvilCorpBakeryAppDbContext : DbContext
    {

    public EvilCorpBakeryAppDbContext(DbContextOptions<EvilCorpBakeryAppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PaymentTypes> PaymentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ========================================
            // USER RELATIONSHIPS
            // ========================================

            // User -> Orders (Restrict - nie można usunąć użytkownika z zamówieniami)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Addresses (Cascade - usuń adresy wraz z użytkownikiem)
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========================================
            // ORDER RELATIONSHIPS
            // ========================================

            // Address -> Orders (Restrict - nie można usunąć adresu używanego w zamówieniach)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderStatus -> Orders (Restrict)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Status)
                .WithMany(os => os.Orders)
                .HasForeignKey(o => o.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // PaymentTypes -> Orders (Restrict)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.PaymentTypes)
                .WithMany()
                .HasForeignKey(o => o.PaymentMethod)
                .OnDelete(DeleteBehavior.Restrict);

            // Order -> OrderItems (Cascade - usuń pozycje wraz z zamówieniem)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========================================
            // PRODUCT RELATIONSHIPS
            // ========================================

            // Category -> Products (Restrict - nie można usunąć kategorii z produktami)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Photos)
                .WithOne(ph => ph.Product)
                .HasForeignKey(ph => ph.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product -> OrderItems (Restrict - nie można usunąć produktu z zamówień)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========================================
            // ADDITIONAL CONFIGURATIONS (opcjonalne)
            // ========================================

            // Subtotal w OrderItem nie jest mapowane do bazy
            modelBuilder.Entity<OrderItem>()
                .Ignore(oi => oi.Subtotal);

            // Indeksy dla lepszej wydajności
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderGuid)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
