using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedHeistAssignedMembersRateBrowse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW vw_heist_assign_members_rate_browse AS
                        WITH heist_assigned_members AS (SELECT h.id, hts.skill_id, COUNT(DISTINCT hm.member_id) AS members
                                    FROM heists h
                                             JOIN heist_members hm ON hm.heist_id = h.id
                                             JOIN member_to_skills mts ON mts.member_id = hm.member_id
                                             JOIN heist_to_skills hts
                                                  ON hts.heist_id = h.id AND mts.skill_id = hts.skill_id AND
                                                     mts.level >= hts.level
                                    GROUP BY h.id, hts.skill_id)
                SELECT hma.heist_id, CASE WHEN hma.required_members IS NULL OR hma.required_members = 0 THEN 0 ELSE ROUND(hma.assigned_members/hma.required_members * 100) END AS assign_rate
                FROM (
                SELECT hts.heist_id, SUM(hts.members) AS required_members,
                       SUM(CASE WHEN COALESCE(ham.members, 0) > hts.members THEN hts.members ELSE COALESCE(ham.members, 0) END) AS assigned_members
                FROM heist_to_skills hts
                LEFT JOIN heist_assigned_members ham ON hts.skill_id = ham.skill_id AND ham.id = hts.heist_id
                GROUP BY hts.heist_id ) AS hma");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
