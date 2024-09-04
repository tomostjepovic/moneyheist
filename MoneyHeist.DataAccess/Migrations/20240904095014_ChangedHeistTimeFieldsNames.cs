using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedHeistTimeFieldsNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "start",
                table: "heists",
                newName: "start_time");

            migrationBuilder.RenameColumn(
                name: "end",
                table: "heists",
                newName: "end_time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "start_time",
                table: "heists",
                newName: "start");

            migrationBuilder.RenameColumn(
                name: "end_time",
                table: "heists",
                newName: "end");
        }
    }
}
