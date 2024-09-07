namespace MoneyHeist.Data.Entities
{
    public class MemberStatus
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsExpired { get; set; }
        public bool IsIncarcerated { get; set; }
        public bool CanParticipateInHeist { get; set; }
    }
}
