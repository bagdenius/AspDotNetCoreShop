using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AspNetCoreCourseProject.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { new Guid("10cd0351-3271-46e9-90fe-0bd3e5f9c214"), 1, "Action" },
                    { new Guid("151eadbb-17db-43a4-b7ab-9de78eb24c15"), 2, "Sci-Fi" },
                    { new Guid("efe086e6-e51a-4ee9-bd8d-caf2c60f55a1"), 3, "History" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("10cd0351-3271-46e9-90fe-0bd3e5f9c214"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("151eadbb-17db-43a4-b7ab-9de78eb24c15"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("efe086e6-e51a-4ee9-bd8d-caf2c60f55a1"));
        }
    }
}
