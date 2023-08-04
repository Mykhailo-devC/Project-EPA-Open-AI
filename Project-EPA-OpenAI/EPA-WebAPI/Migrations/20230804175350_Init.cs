using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPA_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 8, 4, 20, 53, 50, 139, DateTimeKind.Local).AddTicks(4690))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordPool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordPool", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordListWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordList_Id = table.Column<int>(type: "int", nullable: false),
                    Word_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordListWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK__WordListWord__WordList_Id_Key",
                        column: x => x.WordList_Id,
                        principalTable: "WordLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__WordListWord__Word_Id_Key",
                        column: x => x.Word_Id,
                        principalTable: "WordPool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "WordLists",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Fruits" });

            migrationBuilder.InsertData(
                table: "WordPool",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Orange" },
                    { 2, "Apple" },
                    { 3, "Cherry" },
                    { 4, "Strawberry" },
                    { 5, "Peach" }
                });

            migrationBuilder.InsertData(
                table: "WordListWords",
                columns: new[] { "Id", "WordList_Id", "Word_Id" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 1, 3 },
                    { 4, 1, 4 },
                    { 5, 1, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordListWords_Word_Id",
                table: "WordListWords",
                column: "Word_Id");

            migrationBuilder.CreateIndex(
                name: "IX_WordListWords_WordList_Id",
                table: "WordListWords",
                column: "WordList_Id");

            migrationBuilder.CreateIndex(
                name: "IX_WordPool_Value",
                table: "WordPool",
                column: "Value",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordListWords");

            migrationBuilder.DropTable(
                name: "WordLists");

            migrationBuilder.DropTable(
                name: "WordPool");
        }
    }
}
