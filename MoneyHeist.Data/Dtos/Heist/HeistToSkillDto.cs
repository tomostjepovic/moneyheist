using System.Text.Json.Serialization;
namespace MoneyHeist.Data.Dtos.Member
{
    public class HeistToSkillDto: SkillLevelDto
    {
        [JsonPropertyName("Members")]
        public int Members { get; set; }

        [JsonIgnore]
        public override string ValidationRegexExpr { get; set; } = @"^\*+$";
    }
}
