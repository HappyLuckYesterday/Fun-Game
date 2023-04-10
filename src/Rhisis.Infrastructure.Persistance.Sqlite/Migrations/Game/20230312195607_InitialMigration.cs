using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhisis.Infrastructure.Persistance.Sqlite.Migrations.Game
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    SerialNumber = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Refine = table.Column<byte>(type: "INTEGER", nullable: true),
                    Element = table.Column<byte>(type: "INTEGER", nullable: true),
                    ElementRefine = table.Column<byte>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.SerialNumber);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Gender = table.Column<byte>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Experience = table.Column<long>(type: "INTEGER", nullable: false, defaultValue: 0L),
                    JobId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Gold = table.Column<int>(type: "INTEGER", nullable: false),
                    Slot = table.Column<byte>(type: "INTEGER", nullable: false),
                    Strength = table.Column<int>(type: "INTEGER", nullable: false),
                    Stamina = table.Column<int>(type: "INTEGER", nullable: false),
                    Dexterity = table.Column<int>(type: "INTEGER", nullable: false),
                    Intelligence = table.Column<int>(type: "INTEGER", nullable: false),
                    Hp = table.Column<int>(type: "INTEGER", nullable: false),
                    Mp = table.Column<int>(type: "INTEGER", nullable: false),
                    Fp = table.Column<int>(type: "INTEGER", nullable: false),
                    SkinSetId = table.Column<int>(type: "INTEGER", nullable: false),
                    HairId = table.Column<int>(type: "INTEGER", nullable: false),
                    HairColor = table.Column<int>(type: "INTEGER", nullable: false),
                    FaceId = table.Column<int>(type: "INTEGER", nullable: false),
                    MapId = table.Column<int>(type: "INTEGER", nullable: false),
                    MapLayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    PosX = table.Column<float>(type: "REAL", nullable: false),
                    PosY = table.Column<float>(type: "REAL", nullable: false),
                    PosZ = table.Column<float>(type: "REAL", nullable: false),
                    Angle = table.Column<float>(type: "REAL", nullable: false, defaultValue: 0f),
                    BankCode = table.Column<int>(type: "INTEGER", nullable: false),
                    StatPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    LastConnectionTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlayTime = table.Column<long>(type: "INTEGER", nullable: false, defaultValue: 0L),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerItems",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    StorageType = table.Column<byte>(type: "INTEGER", nullable: false),
                    Slot = table.Column<byte>(type: "INTEGER", nullable: false),
                    ItemSerialNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerItems", x => new { x.PlayerId, x.StorageType, x.Slot });
                    table.ForeignKey(
                        name: "FK_PlayerItems_Items_ItemSerialNumber",
                        column: x => x.ItemSerialNumber,
                        principalTable: "Items",
                        principalColumn: "SerialNumber");
                    table.ForeignKey(
                        name: "FK_PlayerItems_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_ItemSerialNumber",
                table: "PlayerItems",
                column: "ItemSerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_PlayerId_StorageType_Slot",
                table: "PlayerItems",
                columns: new[] { "PlayerId", "StorageType", "Slot" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
