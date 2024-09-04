using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedViewForEligibleMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW vw_heist_eligible_member_browse AS
                    SELECT DISTINCT v_members.member_id,
                                v_members.heist_id
                    FROM (SELECT m.id                                                        AS member_id,
                                 h.id                                                        AS heist_id,
                                 mts.skill_id,
                                 hts.level                                                   as required_level,
                                 mts.level                                                   as member_level,
                                 h.start_time,
                                 h.end_time,
                                 (SELECT CASE WHEN COUNT(*) > 0 THEN true ELSE false END AS member_is_assigned_to_different_heist_in_same_time
                                  FROM heists h2
                                           JOIN heist_members hm2 ON hm2.heist_id = h2.id
                                  WHERE hm2.member_id = m.id
                                    AND h2.id <> h.id
                                    AND ((h2.start_time BETWEEN h.start_time AND h.end_time) OR
                                         (h2.end_time BETWEEN h.start_time AND h.end_time))) as member_is_assigned_to_different_heist_in_same_time
                          FROM members m
                                   JOIN member_statuses ms
                                        ON ms.id = m.status_id AND ms.can_participate_in_heist = true
                                   JOIN member_to_skills mts ON mts.member_id = m.id
                                   JOIN heist_to_skills hts ON hts.skill_id = mts.skill_id AND hts.level <= mts.level
                                   JOIN heists h ON h.id = hts.heist_id) AS v_members
                WHERE v_members.member_is_assigned_to_different_heist_in_same_time = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
