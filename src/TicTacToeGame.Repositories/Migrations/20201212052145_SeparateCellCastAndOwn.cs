using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTacToeGame.Repositories.Migrations
{
    public partial class SeparateCellCastAndOwn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CastedBy",
                table: "Cell",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CastedBy",
                table: "Cell");
        }
    }
}
