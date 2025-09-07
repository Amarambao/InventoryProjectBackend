using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class CustomDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomIdSequence_inventory_item_types_ItemId_InventoryId",
                table: "CustomIdSequence");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomIdSequence",
                table: "CustomIdSequence");

            migrationBuilder.RenameTable(
                name: "CustomIdSequence",
                newName: "custom_id_sequence");

            migrationBuilder.RenameIndex(
                name: "IX_CustomIdSequence_ItemId_InventoryId",
                table: "custom_id_sequence",
                newName: "IX_custom_id_sequence_ItemId_InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomIdSequence_InventoryId_ItemId",
                table: "custom_id_sequence",
                newName: "IX_custom_id_sequence_InventoryId_ItemId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "stored_items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "stored_items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "stored_items",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IncrementValue",
                table: "custom_id_sequence",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_custom_id_sequence",
                table: "custom_id_sequence",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "custom_description_sequence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    DescripionType = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_description_sequence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_custom_description_sequence_inventory_item_types_ItemId_Inv~",
                        columns: x => new { x.ItemId, x.InventoryId },
                        principalTable: "inventory_item_types",
                        principalColumns: new[] { "ItemId", "InventoryId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stored_item_descriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoredItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    DescriptionType = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    ShortText = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    LongText = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Number = table.Column<int>(type: "integer", nullable: true),
                    Bool = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stored_item_descriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stored_item_descriptions_stored_items_StoredItemId",
                        column: x => x.StoredItemId,
                        principalTable: "stored_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_custom_description_sequence_InventoryId_ItemId",
                table: "custom_description_sequence",
                columns: new[] { "InventoryId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_custom_description_sequence_ItemId_InventoryId",
                table: "custom_description_sequence",
                columns: new[] { "ItemId", "InventoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_stored_item_descriptions_StoredItemId",
                table: "stored_item_descriptions",
                column: "StoredItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_custom_id_sequence_inventory_item_types_ItemId_InventoryId",
                table: "custom_id_sequence",
                columns: new[] { "ItemId", "InventoryId" },
                principalTable: "inventory_item_types",
                principalColumns: new[] { "ItemId", "InventoryId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_custom_id_sequence_inventory_item_types_ItemId_InventoryId",
                table: "custom_id_sequence");

            migrationBuilder.DropTable(
                name: "custom_description_sequence");

            migrationBuilder.DropTable(
                name: "stored_item_descriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_custom_id_sequence",
                table: "custom_id_sequence");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "stored_items");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "stored_items");

            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "stored_items");

            migrationBuilder.DropColumn(
                name: "IncrementValue",
                table: "custom_id_sequence");

            migrationBuilder.RenameTable(
                name: "custom_id_sequence",
                newName: "CustomIdSequence");

            migrationBuilder.RenameIndex(
                name: "IX_custom_id_sequence_ItemId_InventoryId",
                table: "CustomIdSequence",
                newName: "IX_CustomIdSequence_ItemId_InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_custom_id_sequence_InventoryId_ItemId",
                table: "CustomIdSequence",
                newName: "IX_CustomIdSequence_InventoryId_ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomIdSequence",
                table: "CustomIdSequence",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomIdSequence_inventory_item_types_ItemId_InventoryId",
                table: "CustomIdSequence",
                columns: new[] { "ItemId", "InventoryId" },
                principalTable: "inventory_item_types",
                principalColumns: new[] { "ItemId", "InventoryId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
