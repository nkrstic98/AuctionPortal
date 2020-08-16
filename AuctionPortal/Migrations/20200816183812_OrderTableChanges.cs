using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionPortal.Migrations
{
    public partial class OrderTableChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_AspNetUsers_userId1",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_userId1",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "orders");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "orders",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_orders_userId",
                table: "orders",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_AspNetUsers_userId",
                table: "orders",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_AspNetUsers_userId",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_userId",
                table: "orders");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "userId1",
                table: "orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_orders_userId1",
                table: "orders",
                column: "userId1");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_AspNetUsers_userId1",
                table: "orders",
                column: "userId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
