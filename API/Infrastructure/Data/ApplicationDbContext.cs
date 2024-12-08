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
                new Category { CategoryID = 1, CategoryName = "Tình cảm" },
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
                    BookName = "The Throned Mirror",
                    Author = "Tác giả A",
                    Publisher = "Nhà xuất bản A",
                    PublishedDate = new DateTime(2022, 5, 1),
                    Price = 15000m,
                    StockQuantity = 120,
                    Description = "Một câu chuyện về vương quốc bị sụp đổ và cuộc chiến tranh giành ngai vàng.",
                    Categories = "Tình cảm, Kinh dị",
                    ImagePath = "TheThronedMirror.jpg"
                },
                new Book
                {
                    BookID = 2,
                    BookName = "The Sons of the Empire",
                    Author = "Tác giả B",
                    Publisher = "Nhà xuất bản B",
                    PublishedDate = new DateTime(2023, 3, 15),
                    Price = 18000m,
                    StockQuantity = 90,
                    Description = "Hành trình của những người thừa kế đế chế cổ đại trong một thế giới đầy âm mưu.",
                    Categories = "Viễn tưởng, Kinh dị",
                    ImagePath = "TheSonOfTheEmpire.jpg"
                },
                new Book
                {
                    BookID = 3,
                    BookName = "The Born of APLEX",
                    Author = "Tác giả C",
                    Publisher = "Nhà xuất bản C",
                    PublishedDate = new DateTime(2021, 8, 10),
                    Price = 22000m,
                    StockQuantity = 150,
                    Description = "Cuộc cách mạng công nghệ sinh học mang đến sự thay đổi toàn cầu.",
                    Categories = "Khoa học, Viễn tưởng",
                    ImagePath = "TheBornofAPLEX.jpg"
                },
                new Book
                {
                    BookID = 4,
                    BookName = "Ark Forging",
                    Author = "Tác giả D",
                    Publisher = "Nhà xuất bản D",
                    PublishedDate = new DateTime(2024, 1, 5),
                    Price = 20000m,
                    StockQuantity = 110,
                    Description = "Một cuộc phiêu lưu trong thế giới mới, nơi mà sinh tồn là mục tiêu duy nhất.",
                    Categories = "Viễn tưởng, Kinh dị",
                    ImagePath = "ArkForging.jpg"
                },
                new Book
                {
                    BookID = 5,
                    BookName = "2024: Sanctuary",
                    Author = "Tác giả E",
                    Publisher = "Nhà xuất bản E",
                    PublishedDate = new DateTime(2024, 6, 12),
                    Price = 25000m,
                    StockQuantity = 130,
                    Description = "Mọi hy vọng đều dồn vào nơi trú ẩn cuối cùng khi thế giới đang trên bờ vực diệt vong.",
                    Categories = "Khoa học, Viễn tưởng",
                    ImagePath = "2024Sanctuary.jpg"
                },
                new Book
                {
                    BookID = 6,
                    BookName = "Cyber Angle",
                    Author = "Tác giả F",
                    Publisher = "Nhà xuất bản F",
                    PublishedDate = new DateTime(2023, 11, 20),
                    Price = 17000m,
                    StockQuantity = 140,
                    Description = "Khi công nghệ có thể điều khiển cuộc sống, những bí mật ẩn giấu từ lâu sẽ bị phơi bày.",
                    Categories = "Khoa học, Hài hước",
                    ImagePath = "CyberAngle.jpg"
                },
                new Book
                {
                    BookID = 7,
                    BookName = "Mists of Algorab",
                    Author = "Tác giả G",
                    Publisher = "Nhà xuất bản G",
                    PublishedDate = new DateTime(2022, 4, 8),
                    Price = 19000m,
                    StockQuantity = 100,
                    Description = "Chuyến hành trình kỳ bí vào khu rừng nơi mà không ai biết được điều gì đang chờ đợi.",
                    Categories = "Tình cảm, Hài hước",
                    ImagePath = "MistsofAlgorab.jpg"
                },
                new Book
                {
                    BookID = 8,
                    BookName = "The Return of The King",
                    Author = "Tác giả H",
                    Publisher = "Nhà xuất bản H",
                    PublishedDate = new DateTime(2021, 9, 25),
                    Price = 21000m,
                    StockQuantity = 80,
                    Description = "Vị vua cũ trở lại để dẫn dắt vương quốc chống lại quân thù.",
                    Categories = "Tình cảm, Viễn tưởng",
                    ImagePath = "TheReturnofTheKing.jpg"
                },
                new Book
                {
                    BookID = 9,
                    BookName = "The Silmarillion",
                    Author = "Tác giả I",
                    Publisher = "Nhà xuất bản I",
                    PublishedDate = new DateTime(2022, 12, 1),
                    Price = 24000m,
                    StockQuantity = 75,
                    Description = "Lịch sử của những vương quốc cổ xưa và những anh hùng vĩ đại.",
                    Categories = "Viễn tưởng, Kinh dị",
                    ImagePath = "TheSilmarillion.jpg"
                },
                new Book
                {
                    BookID = 10,
                    BookName = "Six Of Crows Book",
                    Author = "Tác giả J",
                    Publisher = "Nhà xuất bản J",
                    PublishedDate = new DateTime(2023, 7, 16),
                    Price = 26000m,
                    StockQuantity = 110,
                    Description = "Một đội ngũ tội phạm với những mục tiêu bất khả thi phải đối mặt với những thử thách khủng khiếp.",
                    Categories = "Kinh dị, Hài hước",
                    ImagePath = "SixOfCrowsBook.jpg"
                },
                new Book
                {
                    BookID = 11,
                    BookName = "The Hobbit",
                    Author = "Tác giả K",
                    Publisher = "Nhà xuất bản K",
                    PublishedDate = new DateTime(2021, 10, 30),
                    Price = 23000m,
                    StockQuantity = 120,
                    Description = "Cuộc phiêu lưu của Bilbo Baggins vào thế giới của những sinh vật kỳ lạ.",
                    Categories = "Viễn tưởng, Tình cảm",
                    ImagePath = "TheHobbit.jpg"
                },
                new Book
                {
                    BookID = 12,
                    BookName = "The Killing Jar",
                    Author = "Tác giả L",
                    Publisher = "Nhà xuất bản L",
                    PublishedDate = new DateTime(2020, 5, 18),
                    Price = 15000m,
                    StockQuantity = 140,
                    Description = "Một câu chuyện kinh dị về tội phạm và những bí mật đen tối.",
                    Categories = "Kinh dị, Hài hước",
                    ImagePath = "TheKillingJar.jpg"
                },
                new Book
                {
                    BookID = 13,
                    BookName = "Ponti",
                    Author = "Tác giả M",
                    Publisher = "Nhà xuất bản M",
                    PublishedDate = new DateTime(2023, 2, 5),
                    Price = 21000m,
                    StockQuantity = 100,
                    Description = "Một câu chuyện đầy cảm xúc về tình yêu, sự mất mát và tìm lại chính mình.",
                    Categories = "Tình cảm, Hài hước",
                    ImagePath = "Ponti.jpg"
                },
                new Book
                {
                    BookID = 14,
                    BookName = "The Memory Of Light",
                    Author = "Tác giả N",
                    Publisher = "Nhà xuất bản N",
                    PublishedDate = new DateTime(2022, 11, 15),
                    Price = 20000m,
                    StockQuantity = 120,
                    Description = "Một câu chuyện về sự khôi phục và chiến đấu với ánh sáng bên trong.",
                    Categories = "Tình cảm, Viễn tưởng",
                    ImagePath = "TheMemoryOfLight.jpg"
                },
                new Book
                {
                    BookID = 15,
                    BookName = "The Stepsisters",
                    Author = "Tác giả O",
                    Publisher = "Nhà xuất bản O",
                    PublishedDate = new DateTime(2021, 1, 10),
                    Price = 19000m,
                    StockQuantity = 130,
                    Description = "Một câu chuyện về sự ganh đua và tình yêu giữa những người chị em.",
                    Categories = "Tình cảm, Kinh dị",
                    ImagePath = "TheStepsisters.jpg"
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
                    OrderStatus = "Completed",
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
                    OrderStatus = "Completed",
                    PaymentMethod = "COD"
                },
                new Order
                {
                    OrderID = 4,
                    CustomerID = 4,
                    OrderDate = new DateTime(2024, 9, 26),
                    OrderStatus = "Completed",
                    PaymentMethod = "COD"

                },
                new Order
                {
                    OrderID = 5,
                    CustomerID = 5,
                    OrderDate = new DateTime(2024, 10, 2),
                    OrderStatus = "Cancelled",
                    PaymentMethod = "MOMO"

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
                    OrderStatus = "Pending",
                    PaymentMethod = "MOMO"
                },
                new Order
                {
                    OrderID = 7,
                    OrderDate = new DateTime(2024, 12, 1),
                    GuestFullName = "Nguyen Van B",
                    GuestEmail = "nguyenvana@example.com",
                    GuestPhone = "0123456789",
                    GuestCCCD = "123456789",
                    GuestAddress = "123 Main St",
                    OrderStatus = "Pending",
                    PaymentMethod = "MOMO"
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
