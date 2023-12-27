using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashPurse.Server.Migrations
{
    /// <inheritdoc />
    public partial class CloseBudgetListAndBudgetListItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "Expenses",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "CloseList",
                table: "BudgetLists",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ConvertedToExpense",
                table: "BudgetListItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "CloseList",
                table: "BudgetLists");

            migrationBuilder.DropColumn(
                name: "ConvertedToExpense",
                table: "BudgetListItems");

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
    }
}
