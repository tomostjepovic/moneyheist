using System.Text.Json.Serialization;

namespace MoneyHeist.Data.Dtos.Member
{
    public class MemberDto
    {
        [JsonIgnore]
        public int? ID { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("sex")]
        public string? Sex { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("mainSkill")]
        public string? MainSkill { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("skills")]
        public List<MemberToSkillDto>? Skills { get; set; }
    }
}
