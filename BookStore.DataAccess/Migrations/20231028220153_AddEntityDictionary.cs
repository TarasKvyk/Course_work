using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityDictionary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryDescrition", "KeyWords", "Name", "Specialization" },
                values: new object[] { 4, "Description dictionary", "Ukraine", "english", "Dictionary specializetion" });

            migrationBuilder.InsertData(
                table: "DictionaryBooks",
                columns: new[] { "Id", "IntoLanguage", "NativeLanguage" },
                values: new object[] { 4, "Ukrainian", "English" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DictionaryBooks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
