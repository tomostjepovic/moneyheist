using System.Text.Json.Serialization;

namespace MoneyHeist.Data.Dtos.Heist
{
    public class AssignMembersToHeistDto
    {
        [JsonPropertyName("members")]
        public List<string> Members { get; set; }
    }
}
