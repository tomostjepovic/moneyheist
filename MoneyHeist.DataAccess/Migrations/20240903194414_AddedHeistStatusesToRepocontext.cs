using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedHeistStatusesToRepocontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_heists_heist_status_status_id",
                table: "heists");

            migrationBuilder.DropPrimaryKey(
                name: "pk_heist_status",
                table: "heist_status");

            migrationBuilder.RenameTable(
                name: "heist_status",
                newName: "heist_statuses");

            migrationBuilder.AddPrimaryKey(
                name: "pk_heist_statuses",
                table: "heist_statuses",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_heists_heist_statuses_status_id",
                table: "heists",
                column: "status_id",
                principalTable: "heist_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_heists_heist_statuses_status_id",
                table: "heists");

            migrationBuilder.DropPrimaryKey(
                name: "pk_heist_statuses",
                table: "heist_statuses");

            migrationBuilder.RenameTable(
                name: "heist_statuses",
                newName: "heist_status");

            migrationBuilder.AddPrimaryKey(
                name: "pk_heist_status",
                table: "heist_status",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_heists_heist_status_status_id",
                table: "heists",
                column: "status_id",
                principalTable: "heist_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
