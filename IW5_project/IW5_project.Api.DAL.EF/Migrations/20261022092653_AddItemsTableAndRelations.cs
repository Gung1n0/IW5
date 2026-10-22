using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IW5_project.Api.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddItemsTableAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Places_PlaceEntityId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_PlaceEntityId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PlaceEntityId",
                table: "Items");

            migrationBuilder.AddColumn<Guid>(
                name: "PlaceId",
                table: "Items",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Items_PlaceId",
                table: "Items",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Places_PlaceId",
                table: "Items",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Places_PlaceId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_PlaceId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Items");

            migrationBuilder.AddColumn<Guid>(
                name: "PlaceEntityId",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_PlaceEntityId",
                table: "Items",
                column: "PlaceEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Places_PlaceEntityId",
                table: "Items",
                column: "PlaceEntityId",
                principalTable: "Places",
                principalColumn: "Id");
        }
    }
}
