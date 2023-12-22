using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashPurse.Server.Migrations
{
    /// <inheritdoc />
    public partial class ConcurrencyTokeAddition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Incomes",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Expenses",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "BudgetLists",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "BudgetListItems",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "BudgetLists");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "BudgetListItems");
        }
    }
}
