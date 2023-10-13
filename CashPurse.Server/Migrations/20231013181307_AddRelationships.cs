using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashPurse.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AspNetUsers_ApplicationUserId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ApplicationUserId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Expenses");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpenseId",
                table: "BudgetLists",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "BudgetLists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLists_ExpenseId",
                table: "BudgetLists",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLists_OwnerId",
                table: "BudgetLists",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetLists_AspNetUsers_OwnerId",
                table: "BudgetLists",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetLists_Expenses_ExpenseId",
                table: "BudgetLists",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AspNetUsers_ExpenseOwnerId",
                table: "Expenses",
                column: "ExpenseOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetLists_AspNetUsers_OwnerId",
                table: "BudgetLists");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetLists_Expenses_ExpenseId",
                table: "BudgetLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AspNetUsers_ExpenseOwnerId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_BudgetLists_ExpenseId",
                table: "BudgetLists");

            migrationBuilder.DropIndex(
                name: "IX_BudgetLists_OwnerId",
                table: "BudgetLists");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "BudgetLists");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "BudgetLists");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Expenses",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ApplicationUserId",
                table: "Expenses",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AspNetUsers_ApplicationUserId",
                table: "Expenses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
