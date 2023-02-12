using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbAnalyzer.Domain.Migrations
{
    /// <inheritdoc />
    public partial class ConfigStringConfiginit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "config");

            migrationBuilder.CreateTable(
                name: "ConnectionStringConfiguration",
                schema: "config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreation = table.Column<DateTime>(type: "SMALLDATETIME", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Value = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    Comment = table.Column<string>(type: "NVARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionStringConfiguration", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionStringConfiguration",
                schema: "config");
        }
    }
}
