using MoneyHeist.Data.Dtos.SkillLevel;
using System.Text.Json.Serialization;
namespace MoneyHeist.Data.Dtos.Heist
{
    public class HeistToSkillDto: SkillLevelDto
    {
        [JsonPropertyName("members")]
        public int Members { get; set; }

        [JsonIgnore]
        public override string ValidationRegexExpr { get; set; } = @"^\*+$";
    }
}
