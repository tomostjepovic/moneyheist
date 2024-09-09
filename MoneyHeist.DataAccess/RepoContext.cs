using Microsoft.EntityFrameworkCore;
using MoneyHeist.Data.Entities;

namespace MoneyHeist.DataAccess
{
    public class RepoContext: DbContext
    {
        public RepoContext() { }
        public RepoContext(string connectionString) : base(new DbContextOptionsBuilder().UseNpgsql(connectionString).Options) { }
        public RepoContext(DbContextOptions options) : base(options) { }
        public DbSet<MemberStatus> MemberStatuses { get; set; }
        public DbSet<MemberToSkill> MemberToSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Heist> Heists { get; set; }
        public DbSet<HeistToSkill> HeistToSkills { get; set; }
        public DbSet<HeistStatus> HeistStatuses { get; set; }
        public DbSet<HeistMember> HeistMembers { get; set; }
        public DbSet<MembeSkillLevelUp> MembeSkillLevelUps { get; set; }
        public DbSet<HeistEligibleMemberBrowse> HeistEligibleMemberBrowse { get; set; }
        public DbSet<HeistAssignedMembersRateBrowse> HeistAssignedMembersRateBrowse { get; set; }
        public DbSet<HeistSkillMemberBrowse> HeistSkillMemberBrowse { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // TODO: define on cascade delete restrict for needed entities

            base.OnModelCreating(builder);

            builder
                .Entity<HeistSkillMemberBrowse>(eb =>
                {
                    eb.HasKey(x => new { x.HeistID, x.MemberID, x.SkillID });
                    eb.ToView("vw_heist_skill_member_browse");
                    eb.HasOne(x => x.MemberSkill).WithMany(x => x.HeistSkillMemberBrowses).HasForeignKey(x => new { x.MemberID, x.SkillID });
                });

            builder
                .Entity<HeistEligibleMemberBrowse>(eb =>
                {
                    eb.HasKey(x => new { x.HeistID, x.MemberID });
                    eb.ToView("vw_heist_eligible_member_browse");
                });

            builder
                .Entity<HeistAssignedMembersRateBrowse>(eb =>
                {
                    eb.HasKey(x => new { x.HeistID });
                    eb.ToView("vw_heist_assign_members_rate_browse");
                });

            builder.Entity<Heist>()
                .HasIndex(x => x.Name).IsUnique();

            builder.Entity<Heist>()
                .HasMany(x => x.Skills)
                .WithOne(x => x.Heist)
                .HasForeignKey(x => x.HeistID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<HeistToSkill>()
                .HasKey(x => new { x.HeistID, x.SkillID, x.Level });
            builder.Entity<HeistToSkill>()
                .HasKey(x => new { x.HeistID, x.SkillID, x.Level });

            builder.Entity<HeistMember>()
                .HasIndex(x => new { x.HeistID, x.MemberID }).IsUnique();

            builder.Entity<Gender>()
                .HasIndex(x => x.Name).IsUnique();

            builder.Entity<MemberStatus>()
                .HasIndex(x => x.Name).IsUnique();

            builder.Entity<Skill>()
                .HasIndex(x => x.Name).IsUnique();

            builder.Entity<Member>()
                .HasIndex(x => x.Email).IsUnique();

            builder.Entity<Member>()
                .HasMany(x => x.Skills)
                .WithOne(x => x.Member)
                .HasForeignKey(x => x.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MemberToSkill>()
                .HasKey(x => new { x.MemberID, x.SkillID });

            builder.Entity<MembeSkillLevelUp>()
                .HasKey(x => new { x.MemberID, x.HeistID, x.SkillID });
        }
    }
}
