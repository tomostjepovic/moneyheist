using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HeistStatusSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO heist_statuses(name, is_planning, is_ready, is_in_progress, is_finished)
                SELECT 'IN_PROGRESS',  false, false, true, false UNION
                    SELECT 'PLANNING',  true, false, false, false UNION
                    SELECT 'READY', false, true, false, false UNION
                    SELECT 'FINISHED', false, false, false, true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
