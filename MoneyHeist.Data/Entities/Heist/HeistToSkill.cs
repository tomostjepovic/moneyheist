using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyHeist.Data.Entities
{
    public class HeistToSkill
    {
        public int HeistID { get; set; }
        [ForeignKey(nameof(HeistID))]
        public Heist Heist { get; set; }
        public int SkillID { get; set; }
        [ForeignKey(nameof(SkillID))]
        public Skill Skill { get; set; }
        [Range(1, 10)]
        public int Level { get; set; }
        public int Members { get; set; }
    }
}
