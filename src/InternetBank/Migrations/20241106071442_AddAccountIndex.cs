using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetBank.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId_CardNumber_ExpireDate_CVV2",
                table: "Accounts",
                columns: new[] { "UserId", "CardNumber", "ExpireDate", "CVV2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Account_UserId_CardNumber_ExpireDate_CVV2",
                table: "Accounts");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");
        }
    }
}
