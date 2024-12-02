using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Version_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Categories = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullInfo = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    GuestFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestCCCD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    CartID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    BookID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.CartID);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    BookID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailID);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookID", "Author", "BookName", "Categories", "Description", "ImagePath", "Price", "PublishedDate", "Publisher", "StockQuantity" },
                values: new object[,]
                {
                    { 1, "Tác giả 1", "Sách 1", "Tình cảm, Hài hước, Kinh dị", "Mô tả về sách này.", "image1.jpg", 20000m, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nhà xuất bản 1", 100 },
                    { 2, "Tác giả 2", "Sách 2", "Viễn tưởng, Hài hước, Kinh dị", "Mô tả về sách này.", "image2.jpg", 22000m, new DateTime(2020, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nhà xuất bản 2", 50 },
                    { 3, "Tác giả 3", "Sách 3", "Khoa học, Kinh dị", "Mô tả về sách này.", "image3.jpg", 25000m, new DateTime(2019, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nhà xuất bản 3", 75 },
                    { 4, "Tác giả 4", "Sách 4", "Tình cảm, Hài hước, Sức khỏe", "Mô tả về sách này.", "image4.jpg", 30000m, new DateTime(2022, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nhà xuất bản 4", 60 },
                    { 5, "Tác giả 5", "Sách 5", "Khoa học, Sức khỏe", "Mô tả về sách này.", "image5.jpg", 15000m, new DateTime(2023, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nhà xuất bản 5", 80 }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Tình cảm" },
                    { 2, "Khoa học" },
                    { 3, "Viễn tưởng" },
                    { 4, "Kinh dị" },
                    { 5, "Hài hước" },
                    { 6, "Sức khỏe" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerID", "Address", "CCCD", "Email", "FullInfo", "FullName", "IsDeleted", "Password", "Phone", "RefreshToken", "RefreshTokenExpiry", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "123 Main St", "123456789123", "nguyenhuuvinh2893@gmail.com", true, "Nguyễn Hữu Vĩnh", false, "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=", "0123456789", null, null, "Manager", "bookstoremanager" },
                    { 2, "456 Elm St", "987654321456", "huuvinhhoctap0903@gmail.com", true, "Nguyen Admin", false, "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=", "0987654321", null, null, "Admin", "bookstoreadmin" },
                    { 3, "789 Oak St", "456789123142", "levanc@example.com", true, "Tran User", false, "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=", "0345678912", null, null, "User", "bookstoreuser" },
                    { 4, "321 Pine St", "654321987425", "phamthid@example.com", true, "Pham Thi D", false, "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=", "0765432198", null, null, "User", "phamthid" },
                    { 5, "987 Maple St", "321654987487", "hoangvane@example.com", true, "Hoang Van E", false, "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=", "0891234567", null, null, "User", "hoangvane" },
                    { 6, "123 Main St", "123456789628", "nguyenvana@example.com", true, "Nguyễn Văn A", true, "2AF4gdHEe420oL99HvtI6pHqfhyoJWbpHPPJ3Nuo5eo=", "0123456789", null, null, "User", "bookstoreuser" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderID", "CustomerID", "GuestAddress", "GuestCCCD", "GuestEmail", "GuestFullName", "GuestPhone", "OrderDate", "OrderStatus", "PaymentMethod" },
                values: new object[,]
                {
                    { 6, null, "123 Main St", "123456789", "nguyenvana@example.com", "Nguyen Van B", "0123456789", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "Card" },
                    { 7, null, "123 Main St", "123456789", "nguyenvana@example.com", "Nguyen Van B", "0123456789", new DateTime(2024, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "Card" }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailID", "BookID", "OrderID", "Price", "Quantity" },
                values: new object[,]
                {
                    { 8, 1, 6, 20000m, 10 },
                    { 9, 4, 7, 30000m, 5 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderID", "CustomerID", "GuestAddress", "GuestCCCD", "GuestEmail", "GuestFullName", "GuestPhone", "OrderDate", "OrderStatus", "PaymentMethod" },
                values: new object[,]
                {
                    { 1, 1, null, null, null, null, null, new DateTime(2024, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", "COD" },
                    { 2, 2, null, null, null, null, null, new DateTime(2024, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "COD" },
                    { 3, 3, null, null, null, null, null, new DateTime(2024, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shipped", "COD" },
                    { 4, 4, null, null, null, null, null, new DateTime(2024, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", "COD" },
                    { 5, 5, null, null, null, null, null, new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cancelled", "COD" }
                });

            migrationBuilder.InsertData(
                table: "ShoppingCarts",
                columns: new[] { "CartID", "BookID", "CustomerID", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 2, 2, 2 },
                    { 3, 3, 3, 1 },
                    { 4, 4, 4, 3 },
                    { 5, 5, 5, 1 }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailID", "BookID", "OrderID", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 20000m, 2 },
                    { 2, 2, 1, 22000m, 1 },
                    { 3, 3, 2, 25000m, 3 },
                    { 4, 4, 2, 30000m, 1 },
                    { 5, 5, 3, 15000m, 5 },
                    { 6, 2, 4, 22000m, 1 },
                    { 7, 3, 5, 25000m, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_BookID",
                table: "OrderDetails",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID",
                table: "OrderDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_BookID",
                table: "ShoppingCarts",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CustomerID",
                table: "ShoppingCarts",
                column: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
