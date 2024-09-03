using MoneyHeist.Data.Dtos.Heist;
using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.Data.Entities;
using MoneyHeist.Data.Models;

namespace MoneyHeist.Application.Interfaces
{
    public interface IHeistService
    {
        public Task<CreateHeistServiceResult> CreateHeist(HeistDto heistDto);
        public Task<HeistDto> GetHeistById(int id);
        public Task<ServiceResult> UpdateHeistSkills(int heistID, HeistSkillsDto updateHeistSkillsDto);
        public bool HeistHasStarted(Heist heist);
    }
}
