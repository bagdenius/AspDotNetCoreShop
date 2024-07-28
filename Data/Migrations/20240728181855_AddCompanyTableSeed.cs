using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyTableSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "City", "Country", "Name", "PhoneNumber", "PostalCode", "State" },
                values: new object[,]
                {
                    { new Guid("0a43b720-b7b4-4e56-9f20-0968c1f1e73c"), "3173 Dye Street", "Chandler", "United States", "DyeS Chandler Co.", "+1(480)-782-1697", "85225", "Arizona" },
                    { new Guid("8c31a04a-ded0-47bd-9005-eb62cbc7a22f"), "3332 Neuport Lane", "Duluth", "United States", "Neuport Lane Tech", "+1(770)-312-8562", "30097", "Georgia" },
                    { new Guid("a5cdcf9c-a679-4498-905b-3104538ede0c"), "3961 Hall Place", "Detroit", "United States", "Hall Place GmBH", "+1(903)-674-5068", "75436", "Texas" },
                    { new Guid("df4bbe9f-b303-4c04-86cb-128c7f0b4ac9"), "17 Masonic Drive", "Allerton", "United States", "Masonic Int", "+1(406)-564-6357", "50008", "Iowa" },
                    { new Guid("e2f94a3f-11dc-4bfd-ae1b-cfc7672706b6"), "4279 Roy Alley", "Greenwood Village", "United States", "Roy Alley Co.", "+1(303)-865-1479", "80111", "Colorado" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("0a43b720-b7b4-4e56-9f20-0968c1f1e73c"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("8c31a04a-ded0-47bd-9005-eb62cbc7a22f"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("a5cdcf9c-a679-4498-905b-3104538ede0c"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("df4bbe9f-b303-4c04-86cb-128c7f0b4ac9"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("e2f94a3f-11dc-4bfd-ae1b-cfc7672706b6"));
        }
    }
}
