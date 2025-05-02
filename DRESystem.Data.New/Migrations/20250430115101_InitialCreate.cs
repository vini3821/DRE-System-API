using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DRESystem.Data.New.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    RegionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.RegionID);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    SectorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.SectorID);
                });

            migrationBuilder.CreateTable(
                name: "CostCenters",
                columns: table => new
                {
                    CCID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FKRegion = table.Column<int>(type: "int", nullable: false),
                    FKSector = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenters", x => x.CCID);
                    table.ForeignKey(
                        name: "FK_CostCenters_Regions_FKRegion",
                        column: x => x.FKRegion,
                        principalTable: "Regions",
                        principalColumn: "RegionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CostCenters_Sectors_FKSector",
                        column: x => x.FKSector,
                        principalTable: "Sectors",
                        principalColumn: "SectorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collaborators",
                columns: table => new
                {
                    CollaboratorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FKCC = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborators", x => x.CollaboratorID);
                    table.ForeignKey(
                        name: "FK_Collaborators_CostCenters_FKCC",
                        column: x => x.FKCC,
                        principalTable: "CostCenters",
                        principalColumn: "CCID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    EntryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FKC = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.EntryID);
                    table.ForeignKey(
                        name: "FK_Entries_Collaborators_FKC",
                        column: x => x.FKC,
                        principalTable: "Collaborators",
                        principalColumn: "CollaboratorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_FKCC",
                table: "Collaborators",
                column: "FKCC");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_FKRegion",
                table: "CostCenters",
                column: "FKRegion");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_FKSector",
                table: "CostCenters",
                column: "FKSector");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_FKC",
                table: "Entries",
                column: "FKC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "Collaborators");

            migrationBuilder.DropTable(
                name: "CostCenters");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Sectors");
        }
    }
}
