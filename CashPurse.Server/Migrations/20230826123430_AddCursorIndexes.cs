using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashPurse.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddCursorIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Expenses_Amount",
                table: "Expenses",
                column: "Amount");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseDate",
                table: "Expenses",
                column: "ExpenseDate");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLists_CreatedAt",
                table: "BudgetLists",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Expenses_Amount",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpenseDate",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_BudgetLists_CreatedAt",
                table: "BudgetLists");
        }
    }
}
