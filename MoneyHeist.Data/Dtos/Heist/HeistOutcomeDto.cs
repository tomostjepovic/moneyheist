using System.Text.Json.Serialization;

namespace MoneyHeist.Data.Dtos.Heist
{
    public class HeistOutcomeDto
    {
        [JsonPropertyName("outcome")]
        public string Outcome { get; set; }        
    }
}
