using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyHeist.Data.Entities
{
    public class Heist
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool MemberLevelUpProcessed { get; set; }
        public int StatusID { get; set; }
        [ForeignKey(nameof(StatusID))]
        public HeistStatus Status { get; set; }
        public List<HeistToSkill> Skills { get; set; }
    }
}
