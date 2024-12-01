using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Version_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullInfo",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 1,
                column: "FullInfo",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 2,
                column: "FullInfo",
                value: "True");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 3,
                column: "FullInfo",
                value: "True");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 4,
                column: "FullInfo",
                value: "True");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 5,
                column: "FullInfo",
                value: "True");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 466, DateTimeKind.Local).AddTicks(6734));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 467, DateTimeKind.Local).AddTicks(4495));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 3,
                column: "OrderDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 467, DateTimeKind.Local).AddTicks(4508));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 4,
                column: "OrderDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 467, DateTimeKind.Local).AddTicks(4509));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 5,
                column: "OrderDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 467, DateTimeKind.Local).AddTicks(4510));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 6,
                column: "OrderDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 467, DateTimeKind.Local).AddTicks(4511));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 7,
                column: "OrderDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 467, DateTimeKind.Local).AddTicks(5168));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 467, DateTimeKind.Local).AddTicks(6965));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 467, DateTimeKind.Local).AddTicks(7339));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 3,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 467, DateTimeKind.Local).AddTicks(7341));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 4,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 467, DateTimeKind.Local).AddTicks(7342));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentID",
                keyValue: 5,
                column: "PaymentDate",
                value: new DateTime(2024, 11, 28, 20, 4, 55, 467, DateTimeKind.Local).AddTicks(7343));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullInfo",
                table: "Customers");

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
    }
}
