﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyHeist.Data.Entities
{
    public class MemberToSkill
    {
        public int MemberID { get; set; }
        [ForeignKey(nameof(MemberID))]
        public Member Member { get; set; }
        public int SkillID { get; set; }
        [ForeignKey(nameof(SkillID))]
        public Skill Skill { get; set; }
        [Range(1, 10)]
        public int Level { get; set; }

        public List<HeistSkillMemberBrowse> HeistSkillMemberBrowses { get; set; }
    }
}
