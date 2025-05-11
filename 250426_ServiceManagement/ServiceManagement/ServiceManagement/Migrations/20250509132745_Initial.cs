using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServiceManagement.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Servers",
                columns: table => new
                {
                    ServerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Servers", x => x.ServerId);
                });

            migrationBuilder.InsertData(
                table: "T_Servers",
                columns: new[] { "ServerId", "City", "IsOnline", "Name" },
                values: new object[,]
                {
                    { 1, "Toronto", true, "Server1" },
                    { 2, "Toronto", false, "Server2" },
                    { 3, "Toronto", true, "Server3" },
                    { 4, "Toronto", false, "Server4" },
                    { 5, "Montreal", false, "Server5" },
                    { 6, "Montreal", true, "Server6" },
                    { 7, "Montreal", false, "Server7" },
                    { 8, "Ottawa", true, "Server8" },
                    { 9, "Ottawa", false, "Server9" },
                    { 10, "Calgary", false, "Server10" },
                    { 11, "Calgary", false, "Server11" },
                    { 12, "Halifax", false, "Server12" },
                    { 13, "Halifax", false, "Server13" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Servers");
        }
    }
}
