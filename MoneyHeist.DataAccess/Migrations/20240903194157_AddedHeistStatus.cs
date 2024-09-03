using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedHeistStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status_id",
                table: "heists",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "heist_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    is_initial = table.Column<bool>(type: "boolean", nullable: false),
                    is_planning = table.Column<bool>(type: "boolean", nullable: false),
                    is_ready = table.Column<bool>(type: "boolean", nullable: false),
                    is_in_progress = table.Column<bool>(type: "boolean", nullable: false),
                    is_finished = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_heist_status", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_heists_status_id",
                table: "heists",
                column: "status_id");

            migrationBuilder.AddForeignKey(
                name: "fk_heists_heist_status_status_id",
                table: "heists",
                column: "status_id",
                principalTable: "heist_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_heists_heist_status_status_id",
                table: "heists");

            migrationBuilder.DropTable(
                name: "heist_status");

            migrationBuilder.DropIndex(
                name: "ix_heists_status_id",
                table: "heists");

            migrationBuilder.DropColumn(
                name: "status_id",
                table: "heists");
        }
    }
}
