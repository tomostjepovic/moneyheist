﻿using System.Text.Json.Serialization;

namespace MoneyHeist.Data.Dtos.Member
{
    public class MemberSkillsDto
    {
        [JsonPropertyName("skills")]
        public List<MemberToSkillDto>? Skills { get; set; }
        [JsonPropertyName("mainSkill")]
        public string? MainSkill { get; set; }
    }
}
