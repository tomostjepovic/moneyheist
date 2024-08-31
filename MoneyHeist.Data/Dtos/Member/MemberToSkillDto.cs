using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MoneyHeist.Data.Dtos.Member
{
    public class MemberToSkillDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("level")]
        public string? Level { get; set; }
        public bool LevelIsValid
        {
            get
            {
                if (Level == null || Level.Length == 0)
                {
                    return true;
                }

                var regex = new Regex(@"^\*{0,10}$");

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
