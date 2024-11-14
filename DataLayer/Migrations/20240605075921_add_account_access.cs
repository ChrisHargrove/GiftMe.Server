using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class add_account_access : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftIdeas_GiftLists_GiftListSchemaModelId",
                table: "GiftIdeas");

            migrationBuilder.DropForeignKey(
                name: "FK_GiftLists_Accounts_AccountSchemaModelId",
                table: "GiftLists");

            migrationBuilder.DropIndex(
                name: "IX_GiftLists_AccountSchemaModelId",
                table: "GiftLists");

            migrationBuilder.DropIndex(
                name: "IX_GiftIdeas_GiftListSchemaModelId",
                table: "GiftIdeas");

            migrationBuilder.DropColumn(
                name: "AccountSchemaModelId",
                table: "GiftLists");

            migrationBuilder.DropColumn(
                name: "GiftListSchemaModelId",
                table: "GiftIdeas");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccountAccessors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountAccessors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountAccessors_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountAccessors_AccountId",
                table: "AccountAccessors",
                column: "AccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountAccessors");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Accounts");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountSchemaModelId",
                table: "GiftLists",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GiftListSchemaModelId",
                table: "GiftIdeas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiftLists_AccountSchemaModelId",
                table: "GiftLists",
                column: "AccountSchemaModelId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftIdeas_GiftListSchemaModelId",
                table: "GiftIdeas",
                column: "GiftListSchemaModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftIdeas_GiftLists_GiftListSchemaModelId",
                table: "GiftIdeas",
                column: "GiftListSchemaModelId",
                principalTable: "GiftLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftLists_Accounts_AccountSchemaModelId",
                table: "GiftLists",
                column: "AccountSchemaModelId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
