using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.GenTree.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Given = table.Column<string>(type: "text", nullable: false),
                    Family = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    TopPersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    DownPersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelationLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => new { x.TopPersonId, x.DownPersonId });
                    table.ForeignKey(
                        name: "FK_Relationships_People_DownPersonId",
                        column: x => x.DownPersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relationships_People_TopPersonId",
                        column: x => x.TopPersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_DownPersonId",
                table: "Relationships",
                column: "DownPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_TopPersonId",
                table: "Relationships",
                column: "TopPersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relationships");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
