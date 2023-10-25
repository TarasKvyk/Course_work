using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedClassesHierarcyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricalCategories");

            migrationBuilder.RenameColumn(
                name: "Descrition",
                table: "Categories",
                newName: "CategoryDescrition");

            migrationBuilder.CreateTable(
                name: "ChildrenBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PurposeAge = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildrenBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildrenBooks_Categories_Id",
                        column: x => x.Id,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    NativeLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntoLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictionaryBooks_Categories_Id",
                        column: x => x.Id,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FictionBook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LiteraryFormat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FictionBook", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FictionBook_Categories_Id",
                        column: x => x.Id,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoryBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryBooks_Categories_Id",
                        column: x => x.Id,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScientificBook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    KnowledgeBranch = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientificBook", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScientificBook_Categories_Id",
                        column: x => x.Id,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CategoryDescrition",
                value: "HISTORY desc");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryDescrition", "KeyWords" },
                values: new object[] { "Child desc", "Fairy tale" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryDescrition", "IconUrl", "KeyWords" },
                values: new object[] { 2, "HISTORY desc", "", "USA history" });

            migrationBuilder.InsertData(
                table: "ChildrenBooks",
                columns: new[] { "Id", "PurposeAge" },
                values: new object[] { 3, 10 });

            migrationBuilder.InsertData(
                table: "HistoryBooks",
                columns: new[] { "Id", "Period" },
                values: new object[,]
                {
                    { 1, "19" },
                    { 2, "19" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildrenBooks");

            migrationBuilder.DropTable(
                name: "DictionaryBooks");

            migrationBuilder.DropTable(
                name: "FictionBook");

            migrationBuilder.DropTable(
                name: "HistoryBooks");

            migrationBuilder.DropTable(
                name: "ScientificBook");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "CategoryDescrition",
                table: "Categories",
                newName: "Descrition");

            migrationBuilder.CreateTable(
                name: "HistoricalCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricalCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricalCategories_Categories_Id",
                        column: x => x.Id,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descrition",
                value: "History of Ukraine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Descrition", "KeyWords" },
                values: new object[] { "History of USA", "USA history" });

            migrationBuilder.InsertData(
                table: "HistoricalCategories",
                columns: new[] { "Id", "Period" },
                values: new object[,]
                {
                    { 1, "19" },
                    { 3, "19" }
                });
        }
    }
}
