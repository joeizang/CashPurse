using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashPurse.Server.Migrations
{
    /// <inheritdoc />
    public partial class BudgetListMods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "BudgetLists",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "BudgetLists");
        }
    }
}
