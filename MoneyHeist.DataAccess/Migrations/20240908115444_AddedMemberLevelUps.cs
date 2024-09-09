using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedMemberLevelUps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_member_to_skills",
                table: "member_to_skills");

            migrationBuilder.DropIndex(
                name: "ix_member_to_skills_member_id_skill_id",
                table: "member_to_skills");

            migrationBuilder.DropPrimaryKey(
                name: "pk_heist_to_skills",
                table: "heist_to_skills");

            migrationBuilder.DropIndex(
                name: "ix_heist_to_skills_heist_id_skill_id_level",
                table: "heist_to_skills");

            migrationBuilder.DropColumn(
                name: "id",
                table: "member_to_skills");

            migrationBuilder.DropColumn(
                name: "id",
                table: "heist_to_skills");

            migrationBuilder.AddPrimaryKey(
                name: "pk_member_to_skills",
                table: "member_to_skills",
                columns: new[] { "member_id", "skill_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_heist_to_skills",
                table: "heist_to_skills",
                columns: new[] { "heist_id", "skill_id", "level" });

            migrationBuilder.CreateTable(
                name: "membe_skill_level_ups",
                columns: table => new
                {
                    member_id = table.Column<int>(type: "integer", nullable: false),
                    skill_id = table.Column<int>(type: "integer", nullable: false),
                    heist_id = table.Column<int>(type: "integer", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_membe_skill_level_ups", x => new { x.member_id, x.heist_id, x.skill_id });
                    table.ForeignKey(
                        name: "fk_membe_skill_level_ups_heists_heist_id",
                        column: x => x.heist_id,
                        principalTable: "heists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_membe_skill_level_ups_members_member_id",
                        column: x => x.member_id,
                        principalTable: "members",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_membe_skill_level_ups_skills_skill_id",
                        column: x => x.skill_id,
                        principalTable: "skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_membe_skill_level_ups_heist_id",
                table: "membe_skill_level_ups",
                column: "heist_id");

            migrationBuilder.CreateIndex(
                name: "ix_membe_skill_level_ups_skill_id",
                table: "membe_skill_level_ups",
                column: "skill_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "membe_skill_level_ups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_member_to_skills",
                table: "member_to_skills");

            migrationBuilder.DropPrimaryKey(
                name: "pk_heist_to_skills",
                table: "heist_to_skills");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "member_to_skills",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "heist_to_skills",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_member_to_skills",
                table: "member_to_skills",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_heist_to_skills",
                table: "heist_to_skills",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_member_to_skills_member_id_skill_id",
                table: "member_to_skills",
                columns: new[] { "member_id", "skill_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_heist_to_skills_heist_id_skill_id_level",
                table: "heist_to_skills",
                columns: new[] { "heist_id", "skill_id", "level" },
                unique: true);
        }
    }
}
