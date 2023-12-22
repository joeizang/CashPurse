using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashPurse.Server.Migrations
{
    /// <inheritdoc />
    public partial class AppdbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ExpenseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrencyUsed = table.Column<int>(type: "integer", nullable: false),
                    ReceiptUrl = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    ExpenseType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdatedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrencyUsed = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdatedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ListName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ExpenseId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdatedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetLists_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BudgetListItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    BudgetListId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdatedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetListItems_BudgetLists_BudgetListId",
                        column: x => x.BudgetListId,
                        principalTable: "BudgetLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetListItems_BudgetListId",
                table: "BudgetListItems",
                column: "BudgetListId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLists_CreatedAt",
                table: "BudgetLists",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLists_ExpenseId",
                table: "BudgetLists",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_Amount",
                table: "Expenses",
                column: "Amount");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseDate",
                table: "Expenses",
                column: "ExpenseDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetListItems");

            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "BudgetLists");

            migrationBuilder.DropTable(
                name: "Expenses");
        }
    }
}
