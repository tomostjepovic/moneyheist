using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyHeist.Data.Entities
{
    public class HeistMember
    {
        public int ID { get; set; }
        public int HeistID { get; set; }
        [ForeignKey(nameof(HeistID))]
        public Heist Heist { get; set; }
        public int MemberID { get; set; }
        [ForeignKey(nameof(MemberID))]
        public Member Member { get; set; }
    }
}
