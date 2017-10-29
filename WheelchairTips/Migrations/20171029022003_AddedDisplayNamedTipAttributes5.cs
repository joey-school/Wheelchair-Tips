using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WheelchairTips.Migrations
{
    public partial class AddedDisplayNamedTipAttributes5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tip_AspNetUsers_ApplicationUserId",
                table: "Tip");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Tip",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Tip_AspNetUsers_ApplicationUserId",
                table: "Tip",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tip_AspNetUsers_ApplicationUserId",
                table: "Tip");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Tip",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tip_AspNetUsers_ApplicationUserId",
                table: "Tip",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
