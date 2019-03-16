using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.Infrastructure.Migrations.ApplicationDb
{
    public partial class Add_TitleBasic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TitleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TitleBasics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TitleTypeId = table.Column<int>(nullable: true),
                    PrimaryTitle = table.Column<string>(nullable: true),
                    OriginalTitle = table.Column<string>(nullable: true),
                    IsAdult = table.Column<bool>(nullable: false),
                    StartYear = table.Column<int>(nullable: false),
                    EndYear = table.Column<int>(nullable: false),
                    RuntimeMinutes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleBasics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TitleBasics_TitleTypes_TitleTypeId",
                        column: x => x.TitleTypeId,
                        principalTable: "TitleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TitleBasicGenres",
                columns: table => new
                {
                    TitleBasicId = table.Column<int>(nullable: false),
                    GenreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleBasicGenres", x => new { x.TitleBasicId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_TitleBasicGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TitleBasicGenres_TitleBasics_TitleBasicId",
                        column: x => x.TitleBasicId,
                        principalTable: "TitleBasics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TitleBasicGenres_GenreId",
                table: "TitleBasicGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_TitleBasics_TitleTypeId",
                table: "TitleBasics",
                column: "TitleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TitleBasicGenres");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "TitleBasics");

            migrationBuilder.DropTable(
                name: "TitleTypes");
        }
    }
}
