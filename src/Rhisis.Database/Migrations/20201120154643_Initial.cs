using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rhisis.Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GameItemId = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    Refine = table.Column<sbyte>(type: "TINYINT", nullable: true),
                    Element = table.Column<sbyte>(type: "TINYINT", nullable: true),
                    ElementRefine = table.Column<sbyte>(type: "TINYINT", nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemStorageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemStorageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "NVARCHAR(32)", maxLength: 32, nullable: false),
                    Password = table.Column<string>(type: "VARCHAR(32)", maxLength: 32, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    EmailConfirmed = table.Column<ulong>(type: "BIT", nullable: false, defaultValue: 0ul),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    LastConnectionTime = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Authority = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    IsDeleted = table.Column<ulong>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemAttributes",
                columns: table => new
                {
                    ItemId = table.Column<int>(nullable: false),
                    AttributeId = table.Column<int>(nullable: false),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAttributes", x => new { x.ItemId, x.AttributeId });
                    table.ForeignKey(
                        name: "FK_ItemAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemAttributes_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "NVARCHAR(32)", maxLength: 32, nullable: false),
                    Gender = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    Level = table.Column<int>(nullable: false, defaultValue: 1),
                    Experience = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L),
                    JobId = table.Column<sbyte>(type: "TINYINT", nullable: false, defaultValue: (sbyte)0),
                    Gold = table.Column<int>(nullable: false),
                    Slot = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    Strength = table.Column<short>(type: "SMALLINT", nullable: false),
                    Stamina = table.Column<short>(type: "SMALLINT", nullable: false),
                    Dexterity = table.Column<short>(type: "SMALLINT", nullable: false),
                    Intelligence = table.Column<short>(type: "SMALLINT", nullable: false),
                    Hp = table.Column<int>(nullable: false),
                    Mp = table.Column<int>(nullable: false),
                    Fp = table.Column<int>(nullable: false),
                    SkinSetId = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    HairId = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    HairColor = table.Column<int>(nullable: false),
                    FaceId = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    MapId = table.Column<int>(nullable: false),
                    MapLayerId = table.Column<int>(nullable: false),
                    PosX = table.Column<float>(nullable: false),
                    PosY = table.Column<float>(nullable: false),
                    PosZ = table.Column<float>(nullable: false),
                    Angle = table.Column<float>(nullable: false, defaultValue: 0f),
                    BankCode = table.Column<short>(type: "SMALLINT(4)", nullable: false),
                    StatPoints = table.Column<short>(type: "SMALLINT", nullable: false),
                    SkillPoints = table.Column<short>(type: "SMALLINT", nullable: false),
                    LastConnectionTime = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    PlayTime = table.Column<long>(type: "BIGINT", nullable: false, defaultValue: 0L),
                    IsDeleted = table.Column<ulong>(type: "BIT", nullable: false, defaultValue: 0ul),
                    ClusterId = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemStorage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StorageTypeId = table.Column<int>(nullable: false),
                    CharacterId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    Slot = table.Column<short>(type: "SMALLINT", nullable: false),
                    Quantity = table.Column<short>(type: "SMALLINT", nullable: false),
                    Updated = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "NOW()"),
                    IsDeleted = table.Column<ulong>(type: "BIT", nullable: false, defaultValue: 0ul)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemStorage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemStorage_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemStorage_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemStorage_ItemStorage_StorageTypeId",
                        column: x => x.StorageTypeId,
                        principalTable: "ItemStorage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Gold = table.Column<long>(type: "BIGINT", nullable: false),
                    ItemId = table.Column<int>(nullable: true),
                    ItemQuantity = table.Column<short>(nullable: false),
                    HasBeenRead = table.Column<bool>(nullable: false),
                    HasReceivedItem = table.Column<bool>(nullable: false),
                    HasReceivedGold = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    SenderId = table.Column<int>(nullable: false),
                    ReceiverId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mails_Characters_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mails_Characters_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuestId = table.Column<int>(nullable: false),
                    Finished = table.Column<ulong>(type: "BIT", nullable: false, defaultValue: 0ul),
                    IsChecked = table.Column<ulong>(type: "BIT", nullable: false, defaultValue: 0ul),
                    IsDeleted = table.Column<ulong>(type: "BIT", nullable: false, defaultValue: 0ul),
                    StartTime = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    IsPatrolDone = table.Column<ulong>(type: "BIT", nullable: false, defaultValue: 0ul),
                    MonsterKilled1 = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    MonsterKilled2 = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    CharacterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quests_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

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
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SkillId = table.Column<int>(nullable: false),
                    Level = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    CharacterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskbarShortcuts",
                columns: table => new
                {
                    Slot = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    SlotLevelIndex = table.Column<short>(type: "SMALLINT", nullable: false),
                    CharacterId = table.Column<int>(nullable: false),
                    TargetTaskbar = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    Type = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    ObjectType = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    ObjectItemSlot = table.Column<short>(type: "SMALLINT", nullable: true),
                    ObjectIndex = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    UserId = table.Column<short>(type: "SMALLINT", nullable: false),
                    ObjectData = table.Column<short>(type: "SMALLINT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskbarShortcuts", x => new { x.CharacterId, x.Slot, x.SlotLevelIndex });
                    table.ForeignKey(
                        name: "FK_TaskbarShortcuts_Characters_CharacterId",
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

            migrationBuilder.InsertData(
                table: "Attributes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "STR" },
                    { 85, "ONEHANDMASTER_DMG" },
                    { 83, "ATKPOWER" },
                    { 82, "HEAL" },
                    { 81, "MELEE_STEALHP" },
                    { 80, "PVP_DMG" },
                    { 79, "MONSTER_DMG" },
                    { 78, "SKILL_LEVEL" },
                    { 77, "CRITICAL_BONUS" },
                    { 76, "CAST_CRITICAL_RATE" },
                    { 75, "SPELL_RATE" },
                    { 74, "FP_DEC_RATE" },
                    { 86, "TWOHANDMASTER_DMG" },
                    { 73, "MP_DEC_RATE" },
                    { 71, "RECOVERY_EXP" },
                    { 70, "CHR_CHANCEBLEEDING" },
                    { 69, "CHR_CHANCESTEALHP" },
                    { 68, "JUMPING" },
                    { 67, "EXPERIENCE" },
                    { 66, "ATKPOWER_RATE" },
                    { 65, "PARRY" },
                    { 64, "CHRSTATE" },
                    { 63, "CHR_DMG" },
                    { 62, "ADDMAGIC" },
                    { 61, "IMMUNITY" },
                    { 72, "ADJDEF_RATE" },
                    { 60, "CHR_CHANCEPOISON" },
                    { 87, "YOYOMASTER_DMG" },
                    { 89, "KNUCKLEMASTER_DMG" },
                    { 10019, "ALL_DEC_RATE" },
                    { 10018, "KILL_ALL_RATE" },
                    { 10017, "KILL_FP_RATE" },
                    { 10016, "KILL_MP_RATE" },
                    { 10015, "KILL_HP_RATE" },
                    { 10014, "KILL_ALL" },
                    { 10013, "ALL_RECOVERY_RATE" },
                    { 10012, "ALL_RECOVERY" },
                    { 10011, "MASTRY_ALL" },
                    { 10010, "LOCOMOTION" },
                    { 10009, "FP_RECOVERY_RATE" },
                    { 88, "BOWMASTER_DMG" },
                    { 10008, "MP_RECOVERY_RATE" },
                    { 10006, "CURECHR" },
                    { 10005, "DEFHITRATE_DOWN" },
                    { 10004, "HPDMG_UP" },
                    { 10003, "STAT_ALLUP" },
                    { 10002, "RESIST_ALL" },
                    { 10001, "PXP" },
                    { 10000, "GOLD" },
                    { 93, "MAX_ADJPARAMARY" },
                    { 92, "GIFTBOX" },
                    { 91, "RESIST_MAGIC_RATE" },
                    { 90, "HAWKEYE_RATE" },
                    { 10007, "HP_RECOVERY_RATE" },
                    { 58, "AUTOHP" },
                    { 59, "CHR_CHANCEDARK" },
                    { 28, "RESIST_ELECTRICITY" },
                    { 27, "RESIST_MAGIC" },
                    { 26, "ADJDEF" },
                    { 25, "SWD_DMG" },
                    { 24, "ATTACKSPEED" },
                    { 22, "PVP_DMG_RATE" },
                    { 21, "KNUCKLE_DMG" },
                    { 20, "MASTRY_WIND" },
                    { 19, "MASTRY_ELECTRICITY" },
                    { 18, "MASTRY_WATER" },
                    { 17, "MASTRY_FIRE" },
                    { 16, "STOP_MOVEMENT" },
                    { 57, "CHR_CHANCESTUN" },
                    { 15, "MASTRY_EARTH" },
                    { 13, "ABILITY_MAX" },
                    { 12, "ABILITY_MIN" },
                    { 11, "SPEED" },
                    { 10, "CHR_BLEEDING" },
                    { 9, "CHR_CHANCECRITICAL" },
                    { 8, "BLOCK_RANGE" },
                    { 7, "CHR_RANGE" },
                    { 6, "BOW_DMG" },
                    { 5, "YOY_DMG" },
                    { 4, "STA" },
                    { 3, "INT" },
                    { 14, "BLOCK_MELEE" },
                    { 2, "DEX" },
                    { 29, "REFLECT_DAMAGE" },
                    { 31, "RESIST_WIND" },
                    { 56, "CHR_STEALHP" },
                    { 55, "CHR_WEAEATKCHANGE" },
                    { 54, "FP_MAX_RATE" },
                    { 53, "MP_MAX_RATE" },
                    { 52, "HP_MAX_RATE" },
                    { 51, "ATTACKSPEED_RATE" },
                    { 50, "CHR_STEALHP_IMM" },
                    { 49, "CLEARBUFF" },
                    { 47, "ADJ_HITRATE" },
                    { 46, "KILL_FP" },
                    { 45, "KILL_MP" },
                    { 30, "RESIST_FIRE" },
                    { 44, "KILL_HP" },
                    { 42, "MP_RECOVERY" },
                    { 41, "HP_RECOVERY" },
                    { 40, "FP" },
                    { 39, "MP" },
                    { 38, "HP" },
                    { 37, "FP_MAX" },
                    { 36, "MP_MAX" },
                    { 35, "HP_MAX" },
                    { 34, "AXE_DMG" },
                    { 33, "RESIST_EARTH" },
                    { 32, "RESIST_WATER" },
                    { 43, "FP_RECOVERY" }
                });

            migrationBuilder.InsertData(
                table: "ItemStorageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "ExtraBag" },
                    { 3, "Bank" },
                    { 1, "Inventory" },
                    { 4, "GuildBank" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserId",
                table: "Characters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAttributes_AttributeId",
                table: "ItemAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAttributes_ItemId_AttributeId",
                table: "ItemAttributes",
                columns: new[] { "ItemId", "AttributeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemStorage_CharacterId",
                table: "ItemStorage",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStorage_ItemId",
                table: "ItemStorage",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStorage_StorageTypeId",
                table: "ItemStorage",
                column: "StorageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Mails_ItemId",
                table: "Mails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Mails_ReceiverId",
                table: "Mails",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Mails_SenderId",
                table: "Mails",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_CharacterId",
                table: "Quests",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_QuestId_CharacterId",
                table: "Quests",
                columns: new[] { "QuestId", "CharacterId" },
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CharacterId",
                table: "Skills",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillId_CharacterId",
                table: "Skills",
                columns: new[] { "SkillId", "CharacterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskbarShortcuts_CharacterId_Slot_SlotLevelIndex",
                table: "TaskbarShortcuts",
                columns: new[] { "CharacterId", "Slot", "SlotLevelIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username_Email",
                table: "Users",
                columns: new[] { "Username", "Email" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemAttributes");

            migrationBuilder.DropTable(
                name: "ItemStorage");

            migrationBuilder.DropTable(
                name: "ItemStorageTypes");

            migrationBuilder.DropTable(
                name: "Mails");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropTable(
                name: "SkillBuffAttributes");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "TaskbarShortcuts");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "SkillBuffs");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
