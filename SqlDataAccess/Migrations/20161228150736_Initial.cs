using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MMU.BoerseDownloader.SqlDataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Core");

            migrationBuilder.CreateTable(
                name: "BoerseUser",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LoginName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoerseUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DownloadContext",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BoerseLinkProvider = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ThreadUrl = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadContext", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DownloadEntryConfiguration",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DownloadLinkIdentifier = table.Column<string>(nullable: true),
                    IsLinkVisited = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadEntryConfiguration", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoerseUser",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "DownloadContext",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "DownloadEntryConfiguration",
                schema: "Core");
        }
    }
}
