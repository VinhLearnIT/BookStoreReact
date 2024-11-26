using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Version_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiry",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 1,
                columns: new[] { "RefreshToken", "RefreshTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 2,
                columns: new[] { "RefreshToken", "RefreshTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 3,
                columns: new[] { "RefreshToken", "RefreshTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 4,
                columns: new[] { "RefreshToken", "RefreshTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 5,
                columns: new[] { "RefreshToken", "RefreshTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 144, DateTimeKind.Local).AddTicks(1503));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 144, DateTimeKind.Local).AddTicks(9148));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 3,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 144, DateTimeKind.Local).AddTicks(9159));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 4,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 144, DateTimeKind.Local).AddTicks(9161));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 5,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 144, DateTimeKind.Local).AddTicks(9162));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 6,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 144, DateTimeKind.Local).AddTicks(9162));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 7,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 144, DateTimeKind.Local).AddTicks(9847));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 145, DateTimeKind.Local).AddTicks(1660));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 145, DateTimeKind.Local).AddTicks(2093));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 3,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 145, DateTimeKind.Local).AddTicks(2095));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 4,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 145, DateTimeKind.Local).AddTicks(2097));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 5,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 26, 18, 54, 12, 145, DateTimeKind.Local).AddTicks(2098));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiry",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 507, DateTimeKind.Local).AddTicks(6652));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(7834));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 3,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(7854));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 4,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(7856));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 5,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(7857));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 6,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(7858));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 7,
                column: "OrderDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 508, DateTimeKind.Local).AddTicks(8535));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 509, DateTimeKind.Local).AddTicks(592));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 509, DateTimeKind.Local).AddTicks(1171));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 3,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 509, DateTimeKind.Local).AddTicks(1174));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 4,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 509, DateTimeKind.Local).AddTicks(1175));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 5,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 26, 11, 26, 10, 509, DateTimeKind.Local).AddTicks(1177));
        }
    }
}
