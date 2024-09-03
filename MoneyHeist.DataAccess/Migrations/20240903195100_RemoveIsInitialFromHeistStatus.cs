using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsInitialFromHeistStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_initial",
                table: "heist_statuses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_initial",
                table: "heist_statuses",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
