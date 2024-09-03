namespace MoneyHeist.Data.Entities
{
    public class HeistStatus
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsPlanning { get; set; }
        public bool IsReady { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsFinished { get; set; }

    }
}
