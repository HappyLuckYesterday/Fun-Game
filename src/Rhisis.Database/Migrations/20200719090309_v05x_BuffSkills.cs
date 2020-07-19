using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rhisis.Database.Migrations
{
    public partial class v05x_BuffSkills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "ItemsStorage",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.CreateTable(
                name: "SkillBuffs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<int>(nullable: false),
                    SkillId = table.Column<int>(nullable: false),
                    SkillLevel = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    RemainingTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillBuffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillBuffs_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillBuffAttributes",
                columns: table => new
                {
                    SkillBuffId = table.Column<int>(nullable: false),
                    AttributeId = table.Column<int>(nullable: false),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillBuffAttributes", x => new { x.SkillBuffId, x.AttributeId });
                    table.ForeignKey(
                        name: "FK_SkillBuffAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SkillBuffAttributes_SkillBuffs_SkillBuffId",
                        column: x => x.SkillBuffId,
                        principalTable: "SkillBuffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemsStorage_CharacterId",
                table: "ItemsStorage",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillBuffAttributes_AttributeId",
                table: "SkillBuffAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillBuffAttributes_SkillBuffId_AttributeId",
                table: "SkillBuffAttributes",
                columns: new[] { "SkillBuffId", "AttributeId" });

            migrationBuilder.CreateIndex(
                name: "IX_SkillBuffs_CharacterId_SkillId",
                table: "SkillBuffs",
                columns: new[] { "CharacterId", "SkillId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillBuffAttributes");

            migrationBuilder.DropTable(
                name: "SkillBuffs");

            migrationBuilder.DropIndex(
                name: "IX_ItemsStorage_CharacterId",
                table: "ItemsStorage");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "ItemsStorage",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldDefaultValueSql: "NOW()");
        }
    }
}
