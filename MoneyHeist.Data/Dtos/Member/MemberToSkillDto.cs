using System.Text.Json.Serialization;

namespace MoneyHeist.Data.Dtos.Member
{
    public class MemberToSkillDto : SkillLevelDto
    {

        [JsonIgnore]
        public override string ValidationRegexExpr { get; set; } = @"^\*{0,10}$";
    }
}
