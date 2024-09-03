using MoneyHeist.Data.Dtos.Heist;
using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.Data.Entities;

namespace MoneyHeist.Application.Mappers
{
    public static class Mappers
    {

        public static HeistDto ToDto(this Heist entity)
        {
            return new HeistDto
            {
                ID = entity.ID,
                Name = entity.Name,
                StartTime = entity.Start,
                EndTime = entity.End,
                Location = entity.Location,
                Status = entity.Status.Name,
                Skills = entity.Skills.Select(x => x.ToDto()).ToList()
            };
        }
        public static HeistToSkillDto ToDto(this HeistToSkill entity)
        {
            return new HeistToSkillDto
            {
                Name = entity.Skill.Name,
                Members = entity.Members,
                Level = new string('*', entity.Level)
            };
        }

        public static MemberDto ToDto(this Member entity)
        {
            var memberDto = new MemberDto
            {
                ID = entity.ID,
                Name = entity.Name,
                Email = entity.Email,
                MainSkill = entity.MainSkill?.Name,
                Sex = entity.Gender.Name,
                Status = entity.Status.Name,
                Skills = entity.Skills.Select(x => x.ToDto()).ToList()
            };

            return memberDto;
        }

        public static MemberToSkillDto ToDto(this MemberToSkill entity)
        {
            return new MemberToSkillDto
            {
                Name = entity.Skill.Name,
                Level = new string('*', entity.Level)
            };
        }
    }
}
