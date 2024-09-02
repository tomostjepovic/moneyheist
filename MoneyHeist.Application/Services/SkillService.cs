using Microsoft.EntityFrameworkCore;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Data.Entities;
using MoneyHeist.DataAccess;

namespace MoneyHeist.Application.Services
{
    public class SkillService: ISkillService
    {
        private readonly RepoContext repoContext;

        public SkillService(RepoContext _repoContext)
        {
            repoContext = _repoContext;
        }

        public async Task<Skill> GetOrInsertSkillIfNotExists(string name)
        {
            var skill = await repoContext.Skills.SingleOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            if (skill == null)
            {
                skill = new Skill
                {
                    Name = name
                };

                await repoContext.AddAsync(skill);
                await repoContext.SaveChangesAsync();

                return skill;
            }

            return skill;
        }
    }
}
