using EvilCorpBakery.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(EvliCorpBakeryAppDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Products.Any())
            {
                return; // Database has been seeded
            }

            // Seed Products
            var products = new Product[]
            {
                new Product
                {
                    Name = "Chocolate Croissant",
                    Price = 3.50m,
                    Description = "Flaky croissant filled with rich chocolate.",
                    Stock = 25,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o1.png"
                },
                new Product
                {
                    Name = "Sourdough Bread",
                    Price = 5.99m,
                    Description = "Artisan sourdough bread with a crispy crust.",
                    Stock = 15,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o2.png"
                },
                new Product
                {
                    Name = "Blueberry Muffin",
                    Price = 2.75m,
                    Description = "Moist muffin bursting with fresh blueberries.",
                    Stock = 30,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o3.png"
                },
                new Product
                {
                    Name = "Apple Pie",
                    Price = 12.99m,
                    Description = "Classic apple pie with a buttery crust.",
                    Stock = 8,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o4.png"
                },
                new Product
                {
                    Name = "Cinnamon Roll",
                    Price = 4.25m,
                    Description = "Soft roll swirled with cinnamon and topped with icing.",
                    Stock = 20,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o5.png"
                },
                new Product
                {
                    Name = "Bagel",
                    Price = 1.99m,
                    Description = "Freshly baked bagel, perfect with cream cheese.",
                    Stock = 40,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o6.png"
                },
                new Product
                {
                    Name = "Cheesecake Slice",
                    Price = 6.50m,
                    Description = "Creamy cheesecake with a graham cracker crust.",
                    Stock = 12,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o7.png"
                },
                new Product
                {
                    Name = "Danish Pastry",
                    Price = 3.25m,
                    Description = "Buttery pastry filled with fruit or custard.",
                    Stock = 18,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o8.png"
                }
            };
            context.Products.AddRange(products);

            // Seed Users
            var users = new User[]
            {
                new User
                {
                    Email = "admin@evilcorpbakery.com",
                    Password = "Admin123!", // Hash the password
                    Role = "Admin",
                    Name = "System",
                    Surname = "Administrator",
                    DateOfBirth = new DateTime(1990, 1, 1)
                },
                new User
                {
                    Email = "manager@evilcorpbakery.com",
                    Password = "Manager123!",
                    Role = "Admin",
                    Name = "Jane",
                    Surname = "Smith",
                    DateOfBirth = new DateTime(1985, 3, 15)
                },
                new User
                {
                    Email = "john.doe@example.com",
                    Password = "User123!",
                    Role = "User",
                    Name = "John",
                    Surname = "Doe",
                    DateOfBirth = new DateTime(1992, 7, 22)
                },
                new User
                {
                    Email = "test@test.pl",
                    Password = "test",
                    Role = "User",
                    Name = "John",
                    Surname = "Doe",
                    DateOfBirth = new DateTime(1992, 7, 22)
                },
                new User
                {
                    Email = "alice.johnson@example.com",
                    Password = "User123!",
                    Role = "User",
                    Name = "Alice",
                    Surname = "Johnson",
                    DateOfBirth = new DateTime(1988, 11, 10)
                },
                new User
                {
                    Email = "bob.wilson@example.com",
                    Password = "User123!",
                    Role = "User",
                    Name = "Bob",
                    Surname = "Wilson",
                    DateOfBirth = new DateTime(1995, 5, 3)
                }
            };
            context.Users.AddRange(users);

            var categorys = new Category[]
            {
                new Category { Name = "Bread" },
                new Category { Name = "Pastries" },
                new Category { Name = "Cakes" },
                new Category { Name = "Cookies" },
                new Category { Name = "Muffins" }
            };

            // Save all changes
            context.SaveChanges();
        }
    }
}