using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class NormalizedNameIsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tags_NormalizedName",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "IX_item_type_NormalizedName",
                table: "item_type");

            migrationBuilder.DropIndex(
                name: "IX_inventory_type_NormalizedName",
                table: "inventory_type");

            migrationBuilder.CreateIndex(
                name: "IX_tags_NormalizedName",
                table: "tags",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_type_NormalizedName",
                table: "item_type",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventory_type_NormalizedName",
                table: "inventory_type",
                column: "NormalizedName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tags_NormalizedName",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "IX_item_type_NormalizedName",
                table: "item_type");

            migrationBuilder.DropIndex(
                name: "IX_inventory_type_NormalizedName",
                table: "inventory_type");

            migrationBuilder.CreateIndex(
                name: "IX_tags_NormalizedName",
                table: "tags",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_item_type_NormalizedName",
                table: "item_type",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_type_NormalizedName",
                table: "inventory_type",
                column: "NormalizedName");
        }
    }
}
