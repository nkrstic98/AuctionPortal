using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionPortal.Migrations
{
    public partial class UserAuctionsModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_auctions_AspNetUsers_UserId",
                table: "auctions");

            migrationBuilder.DropIndex(
                name: "IX_auctions_UserId",
                table: "auctions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "auctions",
                newName: "userId");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "auctions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userId1",
                table: "auctions",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "auctions",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "auctions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int));


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
    }
}
