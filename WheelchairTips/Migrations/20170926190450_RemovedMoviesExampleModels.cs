using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WheelchairTips.Migrations
{
    public partial class RemovedMoviesExampleModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Tip",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Tip",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Tip",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Tip");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Tip");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Tip");

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.ID);
                });
        }
    }
}
