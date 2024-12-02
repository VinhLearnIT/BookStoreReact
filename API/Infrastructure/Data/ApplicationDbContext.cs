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
                new Category { CategoryID = 1 , CategoryName = "Tình cảm" },
                new Category { CategoryID = 2, CategoryName = "Khoa học" },
                new Category { CategoryID = 3, CategoryName = "Viễn tưởng" },
                new Category { CategoryID = 4, CategoryName = "Kinh dị" },
                new Category { CategoryID = 5, CategoryName = "Hài hước" },
                new Category { CategoryID = 6, CategoryName = "Sức khỏe" }
            );

            // Seed data for Book
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookID = 1,
                    BookName = "Sách 1",
                    Author = "Tác giả 1",
                    Publisher = "Nhà xuất bản 1",
                    PublishedDate = new DateTime(2021, 1, 1),
                    Price = 20000m,
                    StockQuantity = 100,
                    Description = "Mô tả về sách này.",
                    Categories = "Tình cảm, Hài hước, Kinh dị",
                    ImagePath = "image1.jpg"
                },
                new Book
                {
                    BookID = 2,
                    BookName = "Sách 2",
                    Author = "Tác giả 2",
                    Publisher = "Nhà xuất bản 2",
                    PublishedDate = new DateTime(2020, 5, 15),
                    Price = 22000m,
                    StockQuantity = 50,
                    Description = "Mô tả về sách này.",
                    Categories = "Viễn tưởng, Hài hước, Kinh dị",
                    ImagePath = "image2.jpg"
                },
                new Book
                {
                    BookID = 3,
                    BookName = "Sách 3",
                    Author = "Tác giả 3",
                    Publisher = "Nhà xuất bản 3",
                    PublishedDate = new DateTime(2019, 8, 10),
                    Price = 25000m,
                    StockQuantity = 75,
                    Description = "Mô tả về sách này.",
                    Categories = "Khoa học, Kinh dị",
                    ImagePath = "image3.jpg"
                },
                new Book
                {
                    BookID = 4,
                    BookName = "Sách 4",
                    Author = "Tác giả 4",
                    Publisher = "Nhà xuất bản 4",
                    PublishedDate = new DateTime(2022, 3, 12),
                    Price = 30000m,
                    StockQuantity = 60,
                    Description = "Mô tả về sách này.",
                    Categories = "Tình cảm, Hài hước, Sức khỏe",
                    ImagePath = "image4.jpg"
                },
                new Book
                {
                    BookID = 5,
                    BookName = "Sách 5",
                    Author = "Tác giả 5",
                    Publisher = "Nhà xuất bản 5",
                    PublishedDate = new DateTime(2023, 7, 21),
                    Price = 15000m,
                    StockQuantity = 80,
                    Description = "Mô tả về sách này.",
                    Categories = "Khoa học, Sức khỏe",
                    ImagePath = "image5.jpg"
                }
            );

            // Seed data for Customer
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerID = 1,
                    FullName = "Nguyễn Hữu Vĩnh",
                    Email = "nguyenhuuvinh2893@gmail.com",
                    Phone = "0123456789",
                    CCCD = "123456789123",
                    Address = "123 Main St",
                    Username = "bookstoremanager",
                    Password = "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=",
                    Role = "Manager",
                    FullInfo = true,
                    IsDeleted = false
                },
                new Customer
                {
                    CustomerID = 2,
                    FullName = "Nguyen Admin",
                    Email = "huuvinhhoctap0903@gmail.com",
                    Phone = "0987654321",
                    CCCD = "987654321456",
                    Address = "456 Elm St",
                    Username = "bookstoreadmin",
                    Password = "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=",
                    Role = "Admin",
                    FullInfo = true,
                    IsDeleted = false
                },
                new Customer
                {
                    CustomerID = 3,
                    FullName = "Tran User",
                    Email = "levanc@example.com",
                    Phone = "0345678912",
                    CCCD = "456789123142",
                    Address = "789 Oak St",
                    Username = "bookstoreuser",
                    Password = "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=",
                    Role = "User",
                    FullInfo = true,
                    IsDeleted = false
                },
                new Customer
                {
                    CustomerID = 4,
                    FullName = "Pham Thi D",
                    Email = "phamthid@example.com",
                    Phone = "0765432198",
                    CCCD = "654321987425",
                    Address = "321 Pine St",
                    Username = "phamthid",
                    Password = "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=",
                    Role = "User",
                    FullInfo = true,
                    IsDeleted = false
                },
                new Customer
                {
                    CustomerID = 5,
                    FullName = "Hoang Van E",
                    Email = "hoangvane@example.com",
                    Phone = "0891234567",
                    CCCD = "321654987487",
                    Address = "987 Maple St",
                    Username = "hoangvane",
                    Password = "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=",
                    Role = "User",
                    FullInfo = true,
                    IsDeleted = false
                },
                 new Customer
                 {
                     CustomerID = 6,
                     FullName = "Nguyễn Văn A",
                     Email = "nguyenvana@example.com",
                     Phone = "0123456789",
                     CCCD = "123456789628",
                     Address = "123 Main St",
                     Username = "bookstoreuser",
                     Password = "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=",
                     Role = "User",
                     FullInfo = true,
                     IsDeleted = true
                 }
            );

            // Seed data for Order
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    OrderID = 1,
                    CustomerID = 1,
                    OrderDate = new DateTime(2024, 5, 10),
                    OrderStatus = "Pending",
                    PaymentMethod = "COD"
                },
                new Order
                {
                    OrderID = 2,
                    CustomerID = 2,
                    OrderDate = new DateTime(2024, 6, 20),
                    OrderStatus = "Completed",
                    PaymentMethod = "COD"
                },
                new Order
                {
                    OrderID = 3,
                    CustomerID = 3,
                    OrderDate = new DateTime(2024, 8, 11),
                    OrderStatus = "Shipped",
                    PaymentMethod = "COD"
                },
                new Order
                {
                    OrderID = 4,
                    CustomerID = 4,
                    OrderDate = new DateTime(2024, 9, 26),
                    OrderStatus = "Pending",
                    PaymentMethod = "COD"

                },
                new Order
                {
                    OrderID = 5,
                    CustomerID = 5,
                    OrderDate = new DateTime(2024, 10, 2),
                    OrderStatus = "Cancelled",
                    PaymentMethod = "COD"

                },
                new Order
                {
                    OrderID = 6,
                    OrderDate = new DateTime(2024, 11, 1),
                    GuestFullName = "Nguyen Van B",
                    GuestEmail = "nguyenvana@example.com",
                    GuestPhone = "0123456789",
                    GuestCCCD = "123456789",
                    GuestAddress = "123 Main St",
                    OrderStatus = "Completed",
                    PaymentMethod = "Card"
                },
                new Order
                {
                    OrderID = 7,
                    OrderDate = new DateTime(2024, 11, 12),
                    GuestFullName = "Nguyen Van B",
                    GuestEmail = "nguyenvana@example.com",
                    GuestPhone = "0123456789",
                    GuestCCCD = "123456789",
                    GuestAddress = "123 Main St",
                    OrderStatus = "Completed",
                    PaymentMethod = "Card"
                }
            );            

            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail
                {
                    OrderDetailID = 1,
                    OrderID = 1,
                    BookID = 1,
                    Quantity = 2,
                    Price = 20000m
                },
                new OrderDetail
                {
                    OrderDetailID = 2,
                    OrderID = 1,
                    BookID = 2,
                    Quantity = 1,
                    Price = 22000m
                },
                new OrderDetail
                {
                    OrderDetailID = 3,
                    OrderID = 2,
                    BookID = 3,
                    Quantity = 3,
                    Price = 25000m
                },
                new OrderDetail
                {
                    OrderDetailID = 4,
                    OrderID = 2,
                    BookID = 4,
                    Quantity = 1,
                    Price = 30000m
                },
                new OrderDetail
                {
                    OrderDetailID = 5,
                    OrderID = 3,
                    BookID = 5,
                    Quantity = 5,
                    Price = 15000m
                },
                new OrderDetail
                {
                    OrderDetailID = 6,
                    OrderID = 4,
                    BookID = 2,
                    Quantity = 1,
                    Price = 22000m
                },
                new OrderDetail
                {
                    OrderDetailID = 7,
                    OrderID = 5,
                    BookID = 3,
                    Quantity = 7,
                    Price = 25000m
                },
                new OrderDetail
                {
                    OrderDetailID = 8,
                    OrderID = 6,
                    BookID = 1,
                    Quantity = 10,
                    Price = 20000m
                },
                new OrderDetail
                {
                    OrderDetailID = 9,
                    OrderID = 7,
                    BookID = 4,
                    Quantity = 5,
                    Price = 30000m
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
