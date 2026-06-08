using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCloseById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloseById",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CloseById",
                table: "Tickets",
                column: "CloseById");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_CloseById",
                table: "Tickets",
                column: "CloseById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_CloseById",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CloseById",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CloseById",
                table: "Tickets");
        }
    }
}
