using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMyCut.dataa.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Utilisateurs",
                columns: new[] { "Id", "Email", "MotDePasse", "NomComplet", "Role" },
                values: new object[] { 1, "admin@test.com", "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=", "Administrateur", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Utilisateurs",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
