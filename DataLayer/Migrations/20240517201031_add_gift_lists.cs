using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class add_gift_lists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiftLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountSchemaModelId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiftLists_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GiftLists_Accounts_AccountSchemaModelId",
                        column: x => x.AccountSchemaModelId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GiftIdeas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    IsPurchased = table.Column<bool>(type: "boolean", nullable: true),
                    GiftListId = table.Column<Guid>(type: "uuid", nullable: false),
                    GiftListSchemaModelId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftIdeas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiftIdeas_GiftLists_GiftListId",
                        column: x => x.GiftListId,
                        principalTable: "GiftLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GiftIdeas_GiftLists_GiftListSchemaModelId",
                        column: x => x.GiftListSchemaModelId,
                        principalTable: "GiftLists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GiftIdeas_GiftListId",
                table: "GiftIdeas",
                column: "GiftListId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftIdeas_GiftListSchemaModelId",
                table: "GiftIdeas",
                column: "GiftListSchemaModelId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftLists_AccountId",
                table: "GiftLists",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftLists_AccountSchemaModelId",
                table: "GiftLists",
                column: "AccountSchemaModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiftIdeas");

            migrationBuilder.DropTable(
                name: "GiftLists");
        }
    }
}
