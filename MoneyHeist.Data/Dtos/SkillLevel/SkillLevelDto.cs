using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MoneyHeist.Data.Dtos.SkillLevel
{
    public abstract class SkillLevelDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("level")]
        public string? Level { get; set; }

        [JsonIgnore]
        public abstract string ValidationRegexExpr { get; set; }
        [JsonIgnore]
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

        [JsonIgnore]
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
