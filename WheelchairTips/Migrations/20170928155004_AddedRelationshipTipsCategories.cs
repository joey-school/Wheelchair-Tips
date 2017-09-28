using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WheelchairTips.Migrations
{
    public partial class AddedRelationshipTipsCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Tip",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tip_CategoryId",
                table: "Tip",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tip_Category_CategoryId",
                table: "Tip",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tip_Category_CategoryId",
                table: "Tip");

            migrationBuilder.DropIndex(
                name: "IX_Tip_CategoryId",
                table: "Tip");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Tip");
        }
    }
}
