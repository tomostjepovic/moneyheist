using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyHeist.Data.Entities
{
    public class Heist
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int StatusID { get; set; }
        [ForeignKey(nameof(StatusID))]
        public HeistStatus Status { get; set; }

        public List<HeistToSkill> Skills { get; set; }
    }
}
