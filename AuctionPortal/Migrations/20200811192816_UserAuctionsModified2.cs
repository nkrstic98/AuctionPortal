using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionPortal.Migrations
{
    public partial class UserAuctionsModified2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_auctions_AspNetUsers_userId1",
                table: "auctions");

            migrationBuilder.DropIndex(
                name: "IX_auctions_userId1",
                table: "auctions");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "auctions");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "auctions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_auctions_userId",
                table: "auctions",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_auctions_AspNetUsers_userId",
                table: "auctions",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_auctions_AspNetUsers_userId",
                table: "auctions");

            migrationBuilder.DropIndex(
                name: "IX_auctions_userId",
                table: "auctions");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "auctions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "userId1",
                table: "auctions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_auctions_userId1",
                table: "auctions",
                column: "userId1");

            migrationBuilder.AddForeignKey(
                name: "FK_auctions_AspNetUsers_userId1",
                table: "auctions",
                column: "userId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
