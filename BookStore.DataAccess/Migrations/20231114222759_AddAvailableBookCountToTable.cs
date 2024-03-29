﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAvailableBookCountToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableCount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                column: "AvailableCount",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableCount",
                table: "Books");
        }
    }
}
