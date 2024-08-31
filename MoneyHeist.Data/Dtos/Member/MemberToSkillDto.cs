using System.Text.Json.Serialization;

namespace MoneyHeist.Data.Dtos.Member
{
    public class MemberToSkillDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("level")]
        public string? Level { get; set; }
    }
}
