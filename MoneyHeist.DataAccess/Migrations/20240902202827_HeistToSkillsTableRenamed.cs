using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HeistToSkillsTableRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_heist_to_skill_heists_heist_id",
                table: "heist_to_skill");

            migrationBuilder.DropForeignKey(
                name: "fk_heist_to_skill_skills_skill_id",
                table: "heist_to_skill");

            migrationBuilder.DropPrimaryKey(
                name: "pk_heist_to_skill",
                table: "heist_to_skill");

            migrationBuilder.RenameTable(
                name: "heist_to_skill",
                newName: "heist_to_skills");

            migrationBuilder.RenameIndex(
                name: "ix_heist_to_skill_skill_id",
                table: "heist_to_skills",
                newName: "ix_heist_to_skills_skill_id");

            migrationBuilder.RenameIndex(
                name: "ix_heist_to_skill_heist_id_skill_id_level",
                table: "heist_to_skills",
                newName: "ix_heist_to_skills_heist_id_skill_id_level");

            migrationBuilder.AddPrimaryKey(
                name: "pk_heist_to_skills",
                table: "heist_to_skills",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_heist_to_skills_heists_heist_id",
                table: "heist_to_skills",
                column: "heist_id",
                principalTable: "heists",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_heist_to_skills_skills_skill_id",
                table: "heist_to_skills",
                column: "skill_id",
                principalTable: "skills",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_heist_to_skills_heists_heist_id",
                table: "heist_to_skills");

            migrationBuilder.DropForeignKey(
                name: "fk_heist_to_skills_skills_skill_id",
                table: "heist_to_skills");

            migrationBuilder.DropPrimaryKey(
                name: "pk_heist_to_skills",
                table: "heist_to_skills");

            migrationBuilder.RenameTable(
                name: "heist_to_skills",
                newName: "heist_to_skill");

            migrationBuilder.RenameIndex(
                name: "ix_heist_to_skills_skill_id",
                table: "heist_to_skill",
                newName: "ix_heist_to_skill_skill_id");

            migrationBuilder.RenameIndex(
                name: "ix_heist_to_skills_heist_id_skill_id_level",
                table: "heist_to_skill",
                newName: "ix_heist_to_skill_heist_id_skill_id_level");

            migrationBuilder.AddPrimaryKey(
                name: "pk_heist_to_skill",
                table: "heist_to_skill",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_heist_to_skill_heists_heist_id",
                table: "heist_to_skill",
                column: "heist_id",
                principalTable: "heists",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_heist_to_skill_skills_skill_id",
                table: "heist_to_skill",
                column: "skill_id",
                principalTable: "skills",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
