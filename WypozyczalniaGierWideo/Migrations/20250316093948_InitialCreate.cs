using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WypozyczalniaGierWideo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gry",
                columns: table => new
                {
                    IdGry = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tytul = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gatunek = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Platforma = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CenaZaDzien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gry", x => x.IdGry);
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownicy",
                columns: table => new
                {
                    IdUzytkownika = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownicy", x => x.IdUzytkownika);
                });

            migrationBuilder.CreateTable(
                name: "Wypozyczenia",
                columns: table => new
                {
                    IdWypozyczenia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUzytkownika = table.Column<int>(type: "int", nullable: false),
                    IdGry = table.Column<int>(type: "int", nullable: false),
                    DataWypozyczenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataZwrotu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataZwrotuRzeczywista = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Kara = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Koszt = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wypozyczenia", x => x.IdWypozyczenia);
                    table.ForeignKey(
                        name: "FK_Wypozyczenia_Gry_IdGry",
                        column: x => x.IdGry,
                        principalTable: "Gry",
                        principalColumn: "IdGry");
                    table.ForeignKey(
                        name: "FK_Wypozyczenia_Uzytkownicy_IdUzytkownika",
                        column: x => x.IdUzytkownika,
                        principalTable: "Uzytkownicy",
                        principalColumn: "IdUzytkownika",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wypozyczenia_IdGry",
                table: "Wypozyczenia",
                column: "IdGry");

            migrationBuilder.CreateIndex(
                name: "IX_Wypozyczenia_IdUzytkownika",
                table: "Wypozyczenia",
                column: "IdUzytkownika");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wypozyczenia");

            migrationBuilder.DropTable(
                name: "Gry");

            migrationBuilder.DropTable(
                name: "Uzytkownicy");
        }
    }
}
