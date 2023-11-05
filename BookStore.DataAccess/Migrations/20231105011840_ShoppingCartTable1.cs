using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCartTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Books_ProductId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_ProductId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ShoppingCarts");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_BookId",
                table: "ShoppingCarts",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Books_BookId",
                table: "ShoppingCarts",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Books_BookId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_BookId",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ShoppingCarts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ProductId",
                table: "ShoppingCarts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Books_ProductId",
                table: "ShoppingCarts",
                column: "ProductId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
