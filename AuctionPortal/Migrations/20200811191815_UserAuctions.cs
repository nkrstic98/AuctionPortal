using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionPortal.Migrations
{
    public partial class UserAuctions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "auctions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_auctions_UserId",
                table: "auctions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_auctions_AspNetUsers_UserId",
                table: "auctions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_auctions_AspNetUsers_UserId",
                table: "auctions");

            migrationBuilder.DropIndex(
                name: "IX_auctions_UserId",
                table: "auctions");


            migrationBuilder.DropColumn(
                name: "UserId",
                table: "auctions");

            migrationBuilder.DropColumn(
                name: "active",
                table: "AspNetUsers");
        }
    }
}
