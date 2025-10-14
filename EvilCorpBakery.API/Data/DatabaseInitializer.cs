using EvilCorpBakery.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(EvilCorpBakeryAppDbContext context)
        {
            // Ensure the database is created
            //context.Database.EnsureDeleted();  // Usuń
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Products.Any())
            {
                return; // Database has been seeded
            }

            // 1. Seed Categories
            var categories = new Category[]
            {
                new Category { Name = "Bread" },
                new Category { Name = "Pastries" },
                new Category { Name = "Cakes" },
                new Category { Name = "Cookies" },
                new Category { Name = "Muffins" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();


            var paymentTypes = new PaymentTypes[]
            {
                new PaymentTypes { Name = "BLIK" },
                new PaymentTypes { Name = "Credit Card" },
                new PaymentTypes { Name = "PayPal" },
                new PaymentTypes { Name = "Apple Pay" },
                new PaymentTypes { Name = "Google Pay" },
                new PaymentTypes { Name = "Cash on Pickup" }
            };

            context.PaymentTypes.AddRange(paymentTypes);
            context.SaveChanges();


            var orderStatuses = new OrderStatus[]
            {
                new OrderStatus { Name = "Pending", Description = "Order received, awaiting processing" },
                new OrderStatus { Name = "Processing", Description  = "Order is being prepared" },
                new OrderStatus { Name = "Shipped", Description = "Order has been shipped" },
                new OrderStatus { Name = "Completed", Description = "Order completed successfully" },
                new OrderStatus { Name = "Cancelled", Description = "Order was cancelled" }
            };

            context.OrderStatuses.AddRange(orderStatuses);
            context.SaveChanges();


            // 2. Seed Products
            var products = new Product[]
            {
                new Product
                {
                    Name = "Chocolate Croissant",
                    Price = 3.50m,
                    Description = "Flaky croissant filled with rich chocolate.",
                    Stock = 25,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o1.png",
                    CategoryId = 2 // Pastries
                },
                new Product
                {
                    Name = "Sourdough Bread",
                    Price = 5.99m,
                    Description = "Artisan sourdough bread with a crispy crust.",
                    Stock = 15,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o2.png",
                    CategoryId = 1 // Bread
                },
                new Product
                {
                    Name = "Blueberry Muffin",
                    Price = 2.75m,
                    Description = "Moist muffin bursting with fresh blueberries.",
                    Stock = 30,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o3.png",
                    CategoryId = 5 // Muffins
                },
                new Product
                {
                    Name = "Apple Pie",
                    Price = 12.99m,
                    Description = "Classic apple pie with a buttery crust.",
                    Stock = 8,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o4.png",
                    CategoryId = 3 // Cakes
                },
                new Product
                {
                    Name = "Cinnamon Roll",
                    Price = 4.25m,
                    Description = "Soft roll swirled with cinnamon and topped with icing.",
                    Stock = 20,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o5.png",
                    CategoryId = 2 // Pastries
                },
                new Product
                {
                    Name = "Bagel",
                    Price = 1.99m,
                    Description = "Freshly baked bagel, perfect with cream cheese.",
                    Stock = 40,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o6.png",
                    CategoryId = 1 // Bread
                },
                new Product
                {
                    Name = "Cheesecake Slice",
                    Price = 6.50m,
                    Description = "Creamy cheesecake with a graham cracker crust.",
                    Stock = 12,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o7.png",
                    CategoryId = 3 // Cakes
                },
                new Product
                {
                    Name = "Danish Pastry",
                    Price = 3.25m,
                    Description = "Buttery pastry filled with fruit or custard.",
                    Stock = 18,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o8.png",
                    CategoryId = 2 // Pastries
                },
                new Product
                {
                    Name = "Chocolate Chip Cookies",
                    Price = 2.50m,
                    Description = "Classic cookies loaded with chocolate chips.",
                    Stock = 50,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o1.png",
                    CategoryId = 4 // Cookies
                },
                new Product
                {
                    Name = "Banana Bread",
                    Price = 4.99m,
                    Description = "Moist banana bread with walnuts.",
                    Stock = 10,
                    ImageUrl = "C:\\Users\\Michal\\Desktop\\EvilCorpBakery\\Products\\o2.png",
                    CategoryId = 1 // Bread
                }
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            // 3. Seed Users
            var users = new User[]
            {
                new User
                {
                    Email = "admin@evilcorpbakery.com",
                    Password = "Admin123!", // TODO: Hash the password in production
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
                    Name = "Jan",
                    Surname = "Kowalski",
                    DateOfBirth = new DateTime(1995, 5, 10)
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
            context.SaveChanges();

            // 4. Seed Addresses
            var addresses = new Address[]
            {
                // Addresses for John Doe (UserId = 3)
                new Address
                {
                    UserId = 3,
                    Street = "ul. Marszałkowska 123",
                    City = "Warszawa",
                    PostalCode = "00-001",
                    Country = "Poland",
                    PhoneAreaCode = "+48",
                    IsDefault = true,
                    PhoneNumber = "123456789",
                    Label = "Home"
                },
                new Address
                {
                    UserId = 3,
                    Street = "ul. Wspólna 45",
                    City = "Warszawa",
                    PostalCode = "00-002",
                    Country = "Poland",
                    PhoneAreaCode = "+48",
                    IsDefault = false,
                    PhoneNumber = "123456789",
                    Label = "Work"
                },
                // Addresses for Jan Kowalski (UserId = 4)
                new Address
                {
                    UserId = 4,
                    Street = "ul. Kwiatowa 7",
                    City = "Kraków",
                    PostalCode = "30-001",
                    Country = "Poland",
                    PhoneAreaCode = "+48",
                    IsDefault = true,
                    Label = "Home",
                    PhoneNumber = "987654321",
                },
                // Addresses for Alice Johnson (UserId = 5)
                new Address
                {
                    UserId = 5,
                    Street = "ul. Piotrkowska 100",
                    City = "Łódź",
                    PostalCode = "90-001",
                    Country = "Poland",
                    PhoneAreaCode = "+48",
                    IsDefault = true,
                    Label = "Home",
                    PhoneNumber = "555123456"
                },
                // Addresses for Bob Wilson (UserId = 6)
                new Address
                {
                    UserId = 6,
                    Street = "ul. Długa 15",
                    City = "Gdańsk",
                    PostalCode = "80-001",
                    Country = "Poland",
                    PhoneAreaCode = "+48",
                    IsDefault = true,
                    PhoneNumber = "666777888",
                    Label = "Home"
                }
            };
            context.Addresses.AddRange(addresses);
            context.SaveChanges();

            var orders = new Order[]
{
                // Order 1 - John Doe
                new Order
                {
                    UserId = 3,
                    OrderGuid = "#ORD-2024-001",
                    AddressId = 1, // John's home address
                    StatusId = 5,
                    TotalAmount = 15.24m,
                    PaymentMethod = 1,
                    Notes = "Please ring the doorbell twice",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-9)
                },
                // Order 2 - John Doe
                new Order
                {
                    UserId = 3,
                    OrderGuid = "#ORD-2024-002",
                    AddressId = 2, // John's work address
                    StatusId = 2, // Processing
                    TotalAmount = 8.25m,
                    PaymentMethod = 3,
                    Notes = "Delivery between 9-11 AM",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                },
                // Order 3 - Jan Kowalski
                new Order
                {
                    UserId = 4,
                    OrderGuid = "#ORD-2024-003",
                    AddressId = 3,
                    StatusId = 4, 
                    TotalAmount = 22.48m,
                    PaymentMethod = 3,
                    Notes = null,
                    CreatedAt = DateTime.UtcNow.AddHours(-5),
                    UpdatedAt = DateTime.UtcNow.AddHours(-5)
                },
                // Order 4 - Alice Johnson
                new Order
                {
                    UserId = 5,
                    OrderGuid = "#ORD-2024-004",
                    AddressId = 4,
                    StatusId = 4,
                    TotalAmount = 19.99m,
                    PaymentMethod = 5,
                    Notes = "Leave at the door",
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-14)
                },
                // Order 5 - Bob Wilson
                new Order
                {
                    UserId = 6,
                    OrderGuid = "#ORD-2024-005",
                    AddressId = 5,
                    StatusId = 5,
                    TotalAmount = 12.99m,
                    PaymentMethod = 3,
                    Notes = "Changed my mind",
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow.AddDays(-6)
                },
                // Order 6 - John Doe (NEW)
                new Order
                {
                    UserId = 3,
                    OrderGuid = "#ORD-2024-006",
                    AddressId = 1, // John's home address
                    StatusId = 3,
                    TotalAmount = 18.48m,
                    PaymentMethod = 2,
                    Notes = "Call when arriving",
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    UpdatedAt = DateTime.UtcNow.AddDays(-19)
                },
                // Order 7 - John Doe (NEW)
                new Order
                {
                    UserId = 3,
                    OrderGuid = "#ORD-2024-007",
                    AddressId = 1, // John's home address
                    StatusId = 1,
                    TotalAmount = 13.00m,
                    PaymentMethod = 1,
                    Notes = null,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                // Order 8 - John Doe (NEW)
                new Order
                {
                    UserId = 3,
                    OrderGuid = "#ORD-2024-008",
                    AddressId = 2, // John's work address
                    StatusId = 3,
                    TotalAmount = 7.49m,
                    PaymentMethod = 1,
                    Notes = "Urgent delivery",
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow.AddDays(-29)
                }
            };
            context.Orders.AddRange(orders);
            context.SaveChanges();

            // 6. Seed Order Items
            var orderItems = new OrderItem[]
            {
            // Order 1 items (John Doe - Completed)
            new OrderItem
            {
                OrderId = 1,
                ProductId = 1, // Chocolate Croissant
                Quantity = 2,
                UnitPrice = 3.50m
            },
            new OrderItem
            {
                OrderId = 1,
                ProductId = 3, // Blueberry Muffin
                Quantity = 3,
                UnitPrice = 2.75m
            },
    
            // Order 2 items (John Doe - Processing)
            new OrderItem
            {
                OrderId = 2,
                ProductId = 5, // Cinnamon Roll
                Quantity = 2,
                UnitPrice = 4.25m
            },
    
            // Order 3 items (Jan Kowalski - Pending)
            new OrderItem
            {
                OrderId = 3,
                ProductId = 4, // Apple Pie
                Quantity = 1,
                UnitPrice = 12.99m
            },
            new OrderItem
            {
                OrderId = 3,
                ProductId = 9, // Chocolate Chip Cookies
                Quantity = 2,
                UnitPrice = 2.50m
            },
            new OrderItem
            {
                OrderId = 3,
                ProductId = 10, // Banana Bread
                Quantity = 1,
                UnitPrice = 4.99m
            },
    
            // Order 4 items (Alice Johnson - Completed)
            new OrderItem
            {
                OrderId = 4,
                ProductId = 2, // Sourdough Bread
                Quantity = 2,
                UnitPrice = 5.99m
            },
            new OrderItem
            {
                OrderId = 4,
                ProductId = 8, // Danish Pastry
                Quantity = 2,
                UnitPrice = 3.25m
            },
            new OrderItem
            {
                OrderId = 4,
                ProductId = 6, // Bagel
                Quantity = 3,
                UnitPrice = 1.99m
            },
    
            // Order 5 items (Bob Wilson - Cancelled)
            new OrderItem
            {
                OrderId = 5,
                ProductId = 4, // Apple Pie
                Quantity = 1,
                UnitPrice = 12.99m
            },
    
            // Order 6 items (John Doe - Completed) - NEW
            new OrderItem
            {
                OrderId = 6,
                ProductId = 7, // Cheesecake Slice
                Quantity = 2,
                UnitPrice = 6.50m
            },
            new OrderItem
            {
                OrderId = 6,
                ProductId = 2, // Sourdough Bread
                Quantity = 1,
                UnitPrice = 5.99m
            },
    
            // Order 7 items (John Doe - Shipped) - NEW
            new OrderItem
            {
                OrderId = 7,
                ProductId = 7, // Cheesecake Slice
                Quantity = 2,
                UnitPrice = 6.50m
            },
    
            // Order 8 items (John Doe - Completed) - NEW
            new OrderItem
            {
                OrderId = 8,
                ProductId = 9, // Chocolate Chip Cookies
                Quantity = 3,
                UnitPrice = 2.50m
            }
            };
            context.OrderItems.AddRange(orderItems);
            context.SaveChanges();

            Console.WriteLine("Database seeded successfully!");
        }
    }
}