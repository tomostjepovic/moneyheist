namespace MoneyHeist.Data.Entities
{
    public class Heist
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public List<HeistToSkill> Skills { get; set; }
    }
}
