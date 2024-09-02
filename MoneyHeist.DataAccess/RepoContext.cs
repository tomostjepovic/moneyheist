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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Heist>()
                .HasIndex(x => x.Name).IsUnique();

            builder.Entity<HeistToSkill>()
                .HasIndex(x => new { x.HeistID, x.SkillID, x.Level }).IsUnique();

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
