using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyHeist.Data.Entities
{
    public class HeistSkillMemberBrowse
    {
        public int HeistID { get; set; }
        [ForeignKey(nameof(HeistID))]
        public Heist Heist { get; set; }
        public int MemberID { get; set; }
        [ForeignKey(nameof(MemberID))]
        public Member Member { get; set; }
        public int SkillID { get; set; }
        [ForeignKey(nameof(SkillID))]
        public Skill Skill { get; set; }
        public int MemberSkillLevel { get; set; }
        public MemberToSkill MemberSkill { get; set; }
    }
}
