using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bandit.ACS.NpgsqlRepository.Migrations
{
    /// <inheritdoc />
    public partial class m4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionDate",
                table: "Transactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MerchantBank",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MerchantCardNumber",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentToken",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "MerchantBank",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "MerchantCardNumber",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PaymentToken",
                table: "Transactions");
        }
    }
}
