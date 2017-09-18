using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MvcMovie.Migrations
{
    public partial class RenamedTipTableToTips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tip",
                table: "Tip");

            migrationBuilder.RenameTable(
                name: "Tip",
                newName: "Tips");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tips",
                table: "Tips",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tips",
                table: "Tips");

            migrationBuilder.RenameTable(
                name: "Tips",
                newName: "Tip");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tip",
                table: "Tip",
                column: "ID");
        }
    }
}
