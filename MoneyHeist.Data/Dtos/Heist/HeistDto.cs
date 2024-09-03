using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoneyHeist.Data.Dtos.Heist
{
    public class HeistDto
    {
        [JsonIgnore]
        public int? ID { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("location")]
        public string? Location { get; set; }
        [JsonPropertyName("startTime")]
        public DateTime? StartTime { get; set; }
        [JsonPropertyName("endTime")]
        public DateTime? EndTime { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("skills")]
        public List<HeistToSkillDto>? Skills { get; set; }
    }
}
