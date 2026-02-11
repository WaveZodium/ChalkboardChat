using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChalkboardChat.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Messages",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Messages",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Messages",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Messages",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Messages",
                newName: "Date");
        }
    }
}
