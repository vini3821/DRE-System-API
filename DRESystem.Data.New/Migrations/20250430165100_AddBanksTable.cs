using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DRESystem.Data.New.Migrations
{
    /// <inheritdoc />
    public partial class AddBanksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FKBank",
                table: "Entries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_FKBank",
                table: "Entries",
                column: "FKBank");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Banks_FKBank",
                table: "Entries",
                column: "FKBank",
                principalTable: "Banks",
                principalColumn: "BankID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Banks_FKBank",
                table: "Entries");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropIndex(
                name: "IX_Entries_FKBank",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "FKBank",
                table: "Entries");
        }
    }
}
