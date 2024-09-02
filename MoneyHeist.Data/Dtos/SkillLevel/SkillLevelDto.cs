using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MoneyHeist.Data.Dtos.Member
{
    public abstract class SkillLevelDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("level")]
        public string? Level { get; set; }

        [JsonIgnore]
        public abstract string ValidationRegexExpr { get; set; }
        public bool LevelIsValid
        {
            get
            {
                if (Level == null || Level.Length == 0)
                {
                    return true;
                }

                var regex = new Regex(ValidationRegexExpr);

                return regex.IsMatch(Level);
            }
        }

        public int LevelNumeric
        {
            get
            {
                if (Level == null)
                {
                    return 1;
                }

                return Level.Length;
            }
        }
    }
}
