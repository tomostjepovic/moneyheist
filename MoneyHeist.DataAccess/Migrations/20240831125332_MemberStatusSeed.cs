using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MemberStatusSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "can_participate_in_heist",
                table: "member_statuses",
                type: "boolean",
                nullable: false,
                defaultValue: false);


            migrationBuilder.Sql(@"
                INSERT INTO member_statuses (name, can_participate_in_heist)
                    SELECT 'AVAILABLE', true UNION
                    SELECT 'EXPIRED', false UNION
                    SELECT 'INCARCERATED', false UNION
                    SELECT 'RETIRED', true;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "can_participate_in_heist",
                table: "member_statuses");
        }
    }
}
