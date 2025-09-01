using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class InventoryBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inventory_type",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "item_type",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventory_inventory_type_InventoryTypeId",
                        column: x => x.InventoryTypeId,
                        principalTable: "inventory_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat",
                columns: table => new
                {
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WrittenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat", x => new { x.UserId, x.InventoryId, x.WrittenAt });
                    table.ForeignKey(
                        name: "FK_chat_inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_itemss",
                columns: table => new
                {
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "IX_chat_InventoryId",
                table: "chat",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_InventoryTypeId",
                table: "inventory",
                column: "InventoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_itemss_InventoryId",
                table: "inventory_itemss",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_user_inventories_InventoryId",
                table: "user_inventories",
                column: "InventoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat");

            migrationBuilder.DropTable(
                name: "inventory_itemss");

            migrationBuilder.DropTable(
                name: "user_inventories");

            migrationBuilder.DropTable(
                name: "item_type");

            migrationBuilder.DropTable(
                name: "inventory");

            migrationBuilder.DropTable(
                name: "inventory_type");
        }
    }
}
