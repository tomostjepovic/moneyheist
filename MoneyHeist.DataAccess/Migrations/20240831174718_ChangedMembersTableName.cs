using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedMembersTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_member_to_skills_member_member_id",
                table: "member_to_skills");

            migrationBuilder.DropTable(
                name: "member");

            migrationBuilder.CreateTable(
                name: "members",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    gender_id = table.Column<int>(type: "integer", nullable: false),
                    main_skill_id = table.Column<int>(type: "integer", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_members", x => x.id);
                    table.ForeignKey(
                        name: "fk_members_genders_gender_id",
                        column: x => x.gender_id,
                        principalTable: "genders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_members_member_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "member_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_members_skills_main_skill_id",
                        column: x => x.main_skill_id,
                        principalTable: "skills",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_members_email",
                table: "members",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_members_gender_id",
                table: "members",
                column: "gender_id");

            migrationBuilder.CreateIndex(
                name: "ix_members_main_skill_id",
                table: "members",
                column: "main_skill_id");

            migrationBuilder.CreateIndex(
                name: "ix_members_status_id",
                table: "members",
                column: "status_id");

            migrationBuilder.AddForeignKey(
                name: "fk_member_to_skills_members_member_id",
                table: "member_to_skills",
                column: "member_id",
                principalTable: "members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_member_to_skills_members_member_id",
                table: "member_to_skills");

            migrationBuilder.DropTable(
                name: "members");

            migrationBuilder.CreateTable(
                name: "member",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gender_id = table.Column<int>(type: "integer", nullable: false),
                    main_skill_id = table.Column<int>(type: "integer", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_member", x => x.id);
                    table.ForeignKey(
                        name: "fk_member_genders_gender_id",
                        column: x => x.gender_id,
                        principalTable: "genders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_member_member_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "member_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_member_skills_main_skill_id",
                        column: x => x.main_skill_id,
                        principalTable: "skills",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_member_email",
                table: "member",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_member_gender_id",
                table: "member",
                column: "gender_id");

            migrationBuilder.CreateIndex(
                name: "ix_member_main_skill_id",
                table: "member",
                column: "main_skill_id");

            migrationBuilder.CreateIndex(
                name: "ix_member_status_id",
                table: "member",
                column: "status_id");

            migrationBuilder.AddForeignKey(
                name: "fk_member_to_skills_member_member_id",
                table: "member_to_skills",
                column: "member_id",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
