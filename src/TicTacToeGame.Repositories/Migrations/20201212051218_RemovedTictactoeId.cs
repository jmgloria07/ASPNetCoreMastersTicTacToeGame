using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTacToeGame.Repositories.Migrations
{
    public partial class RemovedTictactoeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicTacToeId",
                table: "Games");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicTacToeId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
