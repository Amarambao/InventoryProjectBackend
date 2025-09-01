using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class StoredItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventory_itemss");

            migrationBuilder.DropTable(
                name: "user_inventories");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "NormalizedName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AspNetUsers",
                newName: "Name");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "inventory",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "inventory_editors",
                columns: table => new
                {
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCreator = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_editors", x => new { x.UserId, x.InventoryId });
                    table.ForeignKey(
                        name: "FK_inventory_editors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_editors_inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_item_types",
                columns: table => new
                {
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_item_types", x => new { x.ItemId, x.InventoryId });
                    table.ForeignKey(
                        name: "FK_inventory_item_types_inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_item_types_item_type_ItemId",
                        column: x => x.ItemId,
                        principalTable: "item_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomIdSequence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ElementType = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    FixedTextValue = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomIdSequence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomIdSequence_inventory_item_types_ItemId_InventoryId",
                        columns: x => new { x.ItemId, x.InventoryId },
                        principalTable: "inventory_item_types",
                        principalColumns: new[] { "ItemId", "InventoryId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stored_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stored_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stored_items_inventory_item_types_ItemId_InventoryId",
                        columns: x => new { x.ItemId, x.InventoryId },
                        principalTable: "inventory_item_types",
                        principalColumns: new[] { "ItemId", "InventoryId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_tags",
                columns: table => new
                {
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_tags", x => new { x.InventoryId, x.TagId });
                    table.ForeignKey(
                        name: "FK_inventory_tags_inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_tags_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_item_type_NormalizedName",
                table: "item_type",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_type_NormalizedName",
                table: "inventory_type",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NormalizedName",
                table: "AspNetUsers",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_CustomIdSequence_InventoryId_ItemId",
                table: "CustomIdSequence",
                columns: new[] { "InventoryId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomIdSequence_ItemId_InventoryId",
                table: "CustomIdSequence",
                columns: new[] { "ItemId", "InventoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_inventory_editors_InventoryId",
                table: "inventory_editors",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_item_types_InventoryId",
                table: "inventory_item_types",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_tags_TagId",
                table: "inventory_tags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_stored_items_ItemId_InventoryId",
                table: "stored_items",
                columns: new[] { "ItemId", "InventoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_tags_NormalizedName",
                table: "tags",
                column: "NormalizedName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomIdSequence");

            migrationBuilder.DropTable(
                name: "inventory_editors");

            migrationBuilder.DropTable(
                name: "inventory_tags");

            migrationBuilder.DropTable(
                name: "stored_items");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "inventory_item_types");

            migrationBuilder.DropIndex(
                name: "IX_item_type_NormalizedName",
                table: "item_type");

            migrationBuilder.DropIndex(
                name: "IX_inventory_type_NormalizedName",
                table: "inventory_type");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NormalizedName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "inventory");

            migrationBuilder.RenameColumn(
                name: "NormalizedName",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "FirstName");

            migrationBuilder.CreateTable(
                name: "inventory_itemss",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_itemss", x => new { x.ItemId, x.InventoryId });
                    table.ForeignKey(
                        name: "FK_inventory_itemss_inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_itemss_item_type_ItemId",
                        column: x => x.ItemId,
                        principalTable: "item_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_inventories",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCreator = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_inventories", x => new { x.UserId, x.InventoryId });
                    table.ForeignKey(
                        name: "FK_user_inventories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_inventories_inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inventory_itemss_InventoryId",
                table: "inventory_itemss",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_user_inventories_InventoryId",
                table: "user_inventories",
                column: "InventoryId");
        }
    }
}
