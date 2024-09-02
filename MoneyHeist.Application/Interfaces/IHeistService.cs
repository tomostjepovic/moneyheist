using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.Data.Models;

namespace MoneyHeist.Application.Interfaces
{
    public interface IHeistService
    {
        public Task<CreateHeistServiceResult> CreateHeist(HeistDto heistDto);
    }
}
