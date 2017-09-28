using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WheelchairTips.Migrations
{
    public partial class RenamedPrimaryKeysForTipsAndCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Tip",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Category",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tip",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Category",
                newName: "ID");
        }
    }
}
