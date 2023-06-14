﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonarchsAPI_Net6.Migrations
{
    /// <inheritdoc />
    public partial class AddAverageRatingMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "AverageRating",
                table: "Monarchs",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Monarchs");
        }
    }
}
