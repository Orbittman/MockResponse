using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MockResponse.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    ResponseId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    CacheControl = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ContentEncoding = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    HttpStatusCode = table.Column<int>(nullable: false),
                    Server = table.Column<string>(nullable: true),
                    Vary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.ResponseId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Responses");
        }
    }
}
