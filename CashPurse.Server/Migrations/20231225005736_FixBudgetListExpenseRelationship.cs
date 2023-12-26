using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashPurse.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixBudgetListExpenseRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetLists_Expenses_ExpenseId",
                table: "BudgetLists");

            migrationBuilder.DropIndex(
                name: "IX_BudgetLists_ExpenseId",
                table: "BudgetLists");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "BudgetLists");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetListId",
                table: "Expenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_BudgetListId",
                table: "Expenses",
                column: "BudgetListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_BudgetLists_BudgetListId",
                table: "Expenses",
                column: "BudgetListId",
                principalTable: "BudgetLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_BudgetLists_BudgetListId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_BudgetListId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "BudgetListId",
                table: "Expenses");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpenseId",
                table: "BudgetLists",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLists_ExpenseId",
                table: "BudgetLists",
                column: "ExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetLists_Expenses_ExpenseId",
                table: "BudgetLists",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id");
        }
    }
}
