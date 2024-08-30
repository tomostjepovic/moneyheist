using Microsoft.EntityFrameworkCore;
using MoneyHeist.Data;

namespace MoneyHeist.DataAccess
{
    public class RepoContext: DbContext
    {
        public RepoContext() { }
        public RepoContext(DbContextOptions options) : base(options) { }
        public DbSet<MemberStatus> MemberStatuses { get; set; }
        public DbSet<MemberToSkill> MemberToSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Member> Member { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Gender>()
                .HasIndex(x => x.Name).IsUnique();

            builder.Entity<MemberStatus>()
                .HasIndex(x => x.Name).IsUnique();

            builder.Entity<Skill>()
                .HasIndex(x => x.Name).IsUnique();

            builder.Entity<Member>()
                .HasIndex(x => x.Email).IsUnique();

            builder.Entity<MemberToSkill>()
                .HasIndex(x => new { x.MemberID, x.SkillID }).IsUnique();
        }
    }
}
