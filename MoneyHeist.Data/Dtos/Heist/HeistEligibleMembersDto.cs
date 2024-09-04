using MoneyHeist.Data.Dtos.Member;
using System.Text.Json.Serialization;

namespace MoneyHeist.Data.Dtos.Heist
{
    public class HeistEligibleMembersDto
    {
        [JsonPropertyName("skills")]
        public List<HeistToSkillDto> Skills { get; set; }
        [JsonPropertyName("members")]
        public List<MemberDto> Members { get; set; }
    }
}
