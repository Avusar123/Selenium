using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Selenium.Migrations
{
    public partial class NewUserFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    Balance = table.Column<float>(type: "REAL", nullable: false),
                    NeededCash = table.Column<int>(type: "INTEGER", nullable: false),
                    BetMultiplier = table.Column<int>(type: "INTEGER", nullable: false),
                    AutoStopRatio = table.Column<float>(type: "REAL", nullable: false),
                    LastSpin = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
