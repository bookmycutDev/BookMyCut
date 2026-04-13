using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMyCut.dataa.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTablesSprint2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disponibilites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CoiffeurId = table.Column<int>(type: "INTEGER", nullable: false),
                    Debut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Fin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EstReserve = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disponibilites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disponibilites_Utilisateurs_CoiffeurId",
                        column: x => x.CoiffeurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DureeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Prix = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RendezVous",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    CoiffeurId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateHeure = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Statut = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RendezVous", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RendezVous_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RendezVous_Utilisateurs_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RendezVous_Utilisateurs_CoiffeurId",
                        column: x => x.CoiffeurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Disponibilites_CoiffeurId",
                table: "Disponibilites",
                column: "CoiffeurId");

            migrationBuilder.CreateIndex(
                name: "IX_RendezVous_ClientId",
                table: "RendezVous",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RendezVous_CoiffeurId",
                table: "RendezVous",
                column: "CoiffeurId");

            migrationBuilder.CreateIndex(
                name: "IX_RendezVous_ServiceId",
                table: "RendezVous",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Disponibilites");

            migrationBuilder.DropTable(
                name: "RendezVous");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
