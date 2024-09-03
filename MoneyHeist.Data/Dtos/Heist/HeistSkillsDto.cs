using System.Text.Json.Serialization;

namespace MoneyHeist.Data.Dtos.Heist
{
    public class HeistSkillsDto
    {
        [JsonPropertyName("skills")]
        public List<HeistToSkillDto>? Skills { get; set; }
    }
}
