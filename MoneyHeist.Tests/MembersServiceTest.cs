
using MoneyHeist.Application.Services;
using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.DataAccess;

namespace MoneyHeist.Tests
{
    public class MembersServiceTest
    {
        [Fact]
        public async void MemberNameValidation()
        {
            var repoContext = new RepoContext("Server=localhost;Port=5432;Database=money_heist;Username=postgres;Password=omot0903");
           
            var memberService = new MemberService(repoContext);

            var memberDto = new MemberDto();

            var result = await memberService.CreateMember(memberDto);
            Assert.False(result.Success);
            Assert.Equal("Validation errors", result.ErrorMessage);
            Assert.NotNull(result.Errors);
            Assert.Contains(result.Errors, x => string.Equals("Name is required", x));

        }
    }
}