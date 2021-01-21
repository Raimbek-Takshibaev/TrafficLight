using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrafficLightAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sequences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    StartColor = table.Column<string>(type: "TEXT", nullable: true),
                    BrokenNumbersStr = table.Column<string>(type: "TEXT", nullable: true),
                    NickedBrokenNumbersStr = table.Column<string>(type: "TEXT", nullable: true),
                    StartClocksStr = table.Column<string>(type: "TEXT", nullable: true),
                    IsOver = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartClock = table.Column<int>(type: "INTEGER", nullable: false),
                    CatheringDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MaxClock = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sequences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    SequenceId = table.Column<string>(type: "TEXT", nullable: true),
                    NumbersStr = table.Column<string>(type: "TEXT", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observations_Sequences_SequenceId",
                        column: x => x.SequenceId,
                        principalTable: "Sequences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Observations_SequenceId",
                table: "Observations",
                column: "SequenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "Sequences");
        }
    }
}
