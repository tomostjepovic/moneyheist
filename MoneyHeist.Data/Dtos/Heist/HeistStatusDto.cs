using System.Text.Json.Serialization;

namespace MoneyHeist.Data.Dtos.Heist
{
    public class HeistStatusDto
    {
        [JsonPropertyName("name")]
        public string StatusName { get; set; }
    }
}
