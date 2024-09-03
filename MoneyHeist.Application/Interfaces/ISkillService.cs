using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.Data.Entities;

namespace MoneyHeist.Application.Interfaces
{
    public interface ISkillService
    {
        public Task<Skill> GetOrInsertSkillIfNotExists(string name);
    }
}
