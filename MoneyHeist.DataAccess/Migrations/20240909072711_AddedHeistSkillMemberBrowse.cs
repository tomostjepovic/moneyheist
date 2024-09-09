using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedHeistSkillMemberBrowse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW vw_heist_skill_member_browse AS
                    SELECT hm.heist_id, ms.member_id, hs.skill_id, ms.level AS member_skill_level
                    FROM heist_members hm
                             JOIN members m ON m.id = hm.member_id
                             JOIN member_to_skills ms ON ms.member_id = m.id
                             JOIN heist_to_skills hs ON hs.skill_id = ms.skill_id AND hs.heist_id = hm.heist_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
