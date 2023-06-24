using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhisis.Infrastructure.Persistance.Sqlite.Migrations.Game
{
    /// <inheritdoc />
    public partial class SkillBuffAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerSkillBuffAttributes_PlayerSkillBuffs_PlayerSkillBuffId",
                table: "PlayerSkillBuffAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerSkillBuffs",
                table: "PlayerSkillBuffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerSkillBuffAttributes",
                table: "PlayerSkillBuffAttributes");

            migrationBuilder.RenameColumn(
                name: "PlayerSkillBuffId",
                table: "PlayerSkillBuffAttributes",
                newName: "SkillId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PlayerSkillBuffs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "PlayerSkillBuffAttributes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerSkillBuffs",
                table: "PlayerSkillBuffs",
                columns: new[] { "PlayerId", "SkillId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerSkillBuffAttributes",
                table: "PlayerSkillBuffAttributes",
                columns: new[] { "PlayerId", "SkillId", "Attribute" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerSkillBuffAttributes_PlayerSkillBuffs_PlayerId_SkillId",
                table: "PlayerSkillBuffAttributes",
                columns: new[] { "PlayerId", "SkillId" },
                principalTable: "PlayerSkillBuffs",
                principalColumns: new[] { "PlayerId", "SkillId" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerSkillBuffAttributes_PlayerSkillBuffs_PlayerId_SkillId",
                table: "PlayerSkillBuffAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerSkillBuffs",
                table: "PlayerSkillBuffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerSkillBuffAttributes",
                table: "PlayerSkillBuffAttributes");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "PlayerSkillBuffAttributes");

            migrationBuilder.RenameColumn(
                name: "SkillId",
                table: "PlayerSkillBuffAttributes",
                newName: "PlayerSkillBuffId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PlayerSkillBuffs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerSkillBuffs",
                table: "PlayerSkillBuffs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerSkillBuffAttributes",
                table: "PlayerSkillBuffAttributes",
                columns: new[] { "PlayerSkillBuffId", "Attribute" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerSkillBuffAttributes_PlayerSkillBuffs_PlayerSkillBuffId",
                table: "PlayerSkillBuffAttributes",
                column: "PlayerSkillBuffId",
                principalTable: "PlayerSkillBuffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
