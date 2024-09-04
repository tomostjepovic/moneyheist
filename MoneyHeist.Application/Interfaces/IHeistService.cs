using MoneyHeist.Data.Dtos.Heist;
using MoneyHeist.Data.Entities;
using MoneyHeist.Data.Models;

namespace MoneyHeist.Application.Interfaces
{
    public interface IHeistService
    {
        public Task<CreateHeistServiceResult> CreateHeist(HeistDto heistDto);
        public Task<HeistDto?> GetHeistById(int id);
        public Task<ServiceResult> UpdateHeistSkills(int heistID, HeistSkillsDto updateHeistSkillsDto);
        public Task<ServiceResult> AssignMembersToHeist(int heistID, AssignMembersToHeistDto assignMembersToHeistDto);
        public bool HeistHasStarted(Heist heist);
        public Task<List<HeistToSkillDto>> GetHeistSkills(int id);
        public Task<HeistEligibleMembersServiceResult> GetHeistEligibleMembers(int id);
        public Task<ServiceResult> StartHeist(int id);
        public Task<HeistMembersServiceResult> GetHeistMembers(int id);
        public Task<HeistStatusServiceResult> GetHeistStatus(int id);
        public Task StartHeists();
        public Task FinishHeists();
    }
}
