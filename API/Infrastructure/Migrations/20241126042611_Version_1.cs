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
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderID",
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
                    { 1, "Author A", "Book One", "Fiction, Science, History, Technology, Health", "A great fiction book.", "image1.jpg", 19.99m, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Publisher X", 100 },
                    { 2, "Author B", "Book Two", "Fiction, Science, History, Technology, Health", "An insightful science book.", "image2.jpg", 29.99m, new DateTime(2020, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Publisher Y", 50 },
                    { 3, "Author C", "Book Three", "Fiction, Science, History, Technology, Health", "A detailed history book.", "image3.jpg", 25.00m, new DateTime(2019, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Publisher Z", 75 },
                    { 4, "Author D", "Book Four", "Fiction, Science, History, Technology, Health", "An advanced tech book.", "image4.jpg", 15.50m, new DateTime(2022, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Publisher W", 60 },
                    { 5, "Author E", "Book Five", "Fiction, Science, History, Technology, Health", "A comprehensive health guide.", "image5.jpg", 12.99m, new DateTime(2023, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Publisher V", 80 }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Fiction" },
                    { 2, "Science" },
                    { 3, "History" },
                    { 4, "Technology" },
                    { 5, "Health" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerID", "Address", "CCCD", "Email", "FullName", "Password", "Phone", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "123 Main St", "123456789", "nguyenvana@example.com", "Nguyen Van A", "12345", "0123456789", "User", "bookstoreuser" },
                    { 2, "456 Elm St", "987654321", "huuvinhhoctap0903@gmail.com", "Nguyen Huu Vinh", "12345", "0987654321", "Admin", "bookstoreadmin" },
                    { 3, "789 Oak St", "456789123", "levanc@example.com", "Le Van C", "12345", "0345678912", "User", "levanc" },
                    { 4, "321 Pine St", "654321987", "phamthid@example.com", "Pham Thi D", "12345", "0765432198", "User", "phamthid" },
                    { 5, "987 Maple St", "321654987", "hoangvane@example.com", "Hoang Van E", "12345", "0891234567", "User", "hoangvane" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderID", "CustomerID", "GuestAddress", "GuestCCCD", "GuestEmail", "GuestFullName", "GuestPhone", "OrderDate", "OrderStatus" },
                values: new object[,]
                {
                    { 6, null, "123 Main St", "123456789", "nguyenvana@example.com", "Nguyen Van B", "0123456789", new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(7858), "Completed" },
                    { 7, null, "123 Main St", "123456789", "nguyenvana@example.com", "Nguyen Van B", "0123456789", new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(8535), "Completed" },
                    { 1, 1, null, null, null, null, null, new DateTime(2024, 11, 26, 11, 26, 10, 507, DateTimeKind.Local).AddTicks(6652), "Pending" },
                    { 2, 2, null, null, null, null, null, new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(7834), "Completed" },
                    { 3, 3, null, null, null, null, null, new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(7854), "Shipped" },
                    { 4, 4, null, null, null, null, null, new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(7856), "Pending" },
                    { 5, 5, null, null, null, null, null, new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(7857), "Cancelled" }
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
                    { 1, 1, 1, 39.98m, 2 },
                    { 2, 2, 2, 29.99m, 1 },
                    { 3, 3, 3, 75.00m, 3 },
                    { 4, 4, 4, 15.50m, 1 },
                    { 5, 5, 5, 64.95m, 5 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentID", "OrderID", "PaymentDate", "PaymentMethod", "PaymentStatus" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 11, 26, 11, 26, 10, 509, DateTimeKind.Local).AddTicks(592), "Credit Card", "Paid" },
                    { 2, 2, new DateTime(2024, 11, 26, 11, 26, 10, 509, DateTimeKind.Local).AddTicks(1171), "PayPal", "Paid" },
                    { 3, 3, new DateTime(2024, 11, 26, 11, 26, 10, 509, DateTimeKind.Local).AddTicks(1174), "Bank Transfer", "Pending" },
                    { 4, 4, new DateTime(2024, 11, 26, 11, 26, 10, 509, DateTimeKind.Local).AddTicks(1175), "Cash", "Paid" },
                    { 5, 5, new DateTime(2024, 11, 26, 11, 26, 10, 509, DateTimeKind.Local).AddTicks(1177), "Credit Card", "Failed" }
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
                name: "IX_Payments_OrderID",
                table: "Payments",
                column: "OrderID");

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
                name: "Payments");

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
