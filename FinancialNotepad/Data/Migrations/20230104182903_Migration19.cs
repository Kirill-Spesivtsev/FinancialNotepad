using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialNotepad.Data.Migrations
{
    public partial class Migration19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Credit",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Credit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Credit_CurrencyId",
                table: "Credit",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credit_Currencies_CurrencyId",
                table: "Credit",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credit_Currencies_CurrencyId",
                table: "Credit");

            migrationBuilder.DropIndex(
                name: "IX_Credit_CurrencyId",
                table: "Credit");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Credit");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Credit",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
