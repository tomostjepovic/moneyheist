using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyHeist.Data.Entities
{
    public class Member
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int GenderID { get; set; }
        [ForeignKey(nameof(GenderID))]
        public Gender Gender { get; set; }
        public int? MainSkillID { get; set; }
        [ForeignKey(nameof(MainSkillID))]
        public Skill? MainSkill { get; set; }
        public int StatusID { get; set; }
        [ForeignKey(nameof(StatusID))]
        public MemberStatus Status { get; set; }
    }
}
