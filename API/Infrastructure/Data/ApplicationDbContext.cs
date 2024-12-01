using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for Category
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1 , CategoryName = "Fiction" },
                new Category { CategoryID = 2, CategoryName = "Science" },
                new Category { CategoryID = 3, CategoryName = "History" },
                new Category { CategoryID = 4, CategoryName = "Technology" },
                new Category { CategoryID = 5, CategoryName = "Health" }
            );

            // Seed data for Book
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookID = 1,
                    BookName = "Book One",
                    Author = "Author A",
                    Publisher = "Publisher X",
                    PublishedDate = new DateTime(2021, 1, 1),
                    Price = 19.99m,
                    StockQuantity = 100,
                    Description = "A great fiction book.",
                    Categories = "Fiction, Science, History, Technology, Health",
                    ImagePath = "image1.jpg"
                },
                new Book
                {
                    BookID = 2,
                    BookName = "Book Two",
                    Author = "Author B",
                    Publisher = "Publisher Y",
                    PublishedDate = new DateTime(2020, 5, 15),
                    Price = 29.99m,
                    StockQuantity = 50,
                    Description = "An insightful science book.",
                    Categories = "Fiction, Science, History, Technology, Health",
                    ImagePath = "image2.jpg"
                },
                new Book
                {
                    BookID = 3,
                    BookName = "Book Three",
                    Author = "Author C",
                    Publisher = "Publisher Z",
                    PublishedDate = new DateTime(2019, 8, 10),
                    Price = 25.00m,
                    StockQuantity = 75,
                    Description = "A detailed history book.",
                    Categories = "Fiction, Science, History, Technology, Health",
                    ImagePath = "image3.jpg"
                },
                new Book
                {
                    BookID = 4,
                    BookName = "Book Four",
                    Author = "Author D",
                    Publisher = "Publisher W",
                    PublishedDate = new DateTime(2022, 3, 12),
                    Price = 15.50m,
                    StockQuantity = 60,
                    Description = "An advanced tech book.",
                    Categories = "Fiction, Science, History, Technology, Health",
                    ImagePath = "image4.jpg"
                },
                new Book
                {
                    BookID = 5,
                    BookName = "Book Five",
                    Author = "Author E",
                    Publisher = "Publisher V",
                    PublishedDate = new DateTime(2023, 7, 21),
                    Price = 12.99m,
                    StockQuantity = 80,
                    Description = "A comprehensive health guide.",
                    Categories = "Fiction, Science, History, Technology, Health",
                    ImagePath = "image5.jpg"
                }
            );

            // Seed data for Customer
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerID = 1,
                    FullName = "Nguyen Van A",
                    Email = "nguyenvana@example.com",
                    Phone = "0123456789",
                    CCCD = "123456789",
                    Address = "123 Main St",
                    Username = "bookstoreuser",
                    Password = "12345",
                    Role = "User"
                },
                new Customer
                {
                    CustomerID = 2,
                    FullName = "Nguyen Huu Vinh",
                    Email = "huuvinhhoctap0903@gmail.com",
                    Phone = "0987654321",
                    CCCD = "987654321",
                    Address = "456 Elm St",
                    Username = "bookstoreadmin",
                    Password = "12345",
                    Role = "Admin",
                    FullInfo = "True"
                },
                new Customer
                {
                    CustomerID = 3,
                    FullName = "Le Van C",
                    Email = "levanc@example.com",
                    Phone = "0345678912",
                    CCCD = "456789123",
                    Address = "789 Oak St",
                    Username = "levanc",
                    Password = "12345",
                    Role = "User",
                    FullInfo = "True"
                },
                new Customer
                {
                    CustomerID = 4,
                    FullName = "Pham Thi D",
                    Email = "phamthid@example.com",
                    Phone = "0765432198",
                    CCCD = "654321987",
                    Address = "321 Pine St",
                    Username = "phamthid",
                    Password = "12345",
                    Role = "User",
                    FullInfo = "True"
                },
                new Customer
                {
                    CustomerID = 5,
                    FullName = "Hoang Van E",
                    Email = "hoangvane@example.com",
                    Phone = "0891234567",
                    CCCD = "321654987",
                    Address = "987 Maple St",
                    Username = "hoangvane",
                    Password = "12345",
                    Role = "User",
                    FullInfo = "True"
                }
            );

            // Seed data for Order
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    OrderID = 1,
                    CustomerID = 1,
                    OrderDate = DateTime.Now,
                    OrderStatus = "Pending"
                },
                new Order
                {
                    OrderID = 2,
                    CustomerID = 2,
                    OrderDate = DateTime.Now,
                    OrderStatus = "Completed"
                },
                new Order
                {
                    OrderID = 3,
                    CustomerID = 3,
                    OrderDate = DateTime.Now,
                    OrderStatus = "Shipped"
                },
                new Order
                {
                    OrderID = 4,
                    CustomerID = 4,
                    OrderDate = DateTime.Now,
                    OrderStatus = "Pending"
                },
                new Order
                {
                    OrderID = 5,
                    CustomerID = 5,
                    OrderDate = DateTime.Now,
                    OrderStatus = "Cancelled"
                },
                new Order
                {
                    OrderID = 6,
                    OrderDate = DateTime.Now,
                    GuestFullName = "Nguyen Van B",
                    GuestEmail = "nguyenvana@example.com",
                    GuestPhone = "0123456789",
                    GuestCCCD = "123456789",
                    GuestAddress = "123 Main St",
                    OrderStatus = "Completed"
                },
                new Order
                {
                    OrderID = 7,
                    OrderDate = DateTime.Now,
                    GuestFullName = "Nguyen Van B",
                    GuestEmail = "nguyenvana@example.com",
                    GuestPhone = "0123456789",
                    GuestCCCD = "123456789",
                    GuestAddress = "123 Main St",
                    OrderStatus = "Completed"
                }
            );            

            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail
                {
                    OrderDetailID = 1,
                    OrderID = 1,
                    BookID = 1,
                    Quantity = 2,
                    Price = 39.98m
                },
                new OrderDetail
                {
                    OrderDetailID = 2,
                    OrderID = 2,
                    BookID = 2,
                    Quantity = 1,
                    Price = 29.99m
                },
                new OrderDetail
                {
                    OrderDetailID = 3,
                    OrderID = 3,
                    BookID = 3,
                    Quantity = 3,
                    Price = 75.00m
                },
                new OrderDetail
                {
                    OrderDetailID = 4,
                    OrderID = 4,
                    BookID = 4,
                    Quantity = 1,
                    Price = 15.50m
                },
                new OrderDetail
                {
                    OrderDetailID = 5,
                    OrderID = 5,
                    BookID = 5,
                    Quantity = 5,
                    Price = 64.95m
                }
            );

            // Seed data for Payment
            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    PaymentID = 1,
                    OrderID = 1,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "Credit Card",
                    PaymentStatus = "Paid"
                },
                new Payment
                {
                    PaymentID = 2,
                    OrderID = 2,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "PayPal",
                    PaymentStatus = "Paid"
                },
                new Payment
                {
                    PaymentID = 3,
                    OrderID = 3,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "Bank Transfer",
                    PaymentStatus = "Pending"
                },
                new Payment
                {
                    PaymentID = 4,
                    OrderID = 4,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "Cash",
                    PaymentStatus = "Paid"
                },
                new Payment
                {
                    PaymentID = 5,
                    OrderID = 5,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "Credit Card",
                    PaymentStatus = "Failed"
                }
            );

            // Seed data for ShoppingCart
            modelBuilder.Entity<ShoppingCart>().HasData(
                new ShoppingCart
                {
                    CartID = 1,
                    CustomerID = 1,
                    BookID = 1,
                    Quantity = 1
                },
                new ShoppingCart
                {
                    CartID = 2,
                    CustomerID = 2,
                    BookID = 2,
                    Quantity = 2
                },
                new ShoppingCart
                {
                    CartID = 3,
                    CustomerID = 3,
                    BookID = 3,
                    Quantity = 1
                },
                new ShoppingCart
                {
                    CartID = 4,
                    CustomerID = 4,
                    BookID = 4,
                    Quantity = 3
                },
                new ShoppingCart
                {
                    CartID = 5,
                    CustomerID = 5,
                    BookID = 5,
                    Quantity = 1
                }
            );
        }
    }
}
