using Microsoft.AspNetCore.Mvc;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Data.Dtos.Member;

namespace MoneyHeist.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemberController : ControllerBase
    {
        protected readonly IMemberService memberService;
        public MemberController(IMemberService _memberService)
        {
            memberService = _memberService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync([FromBody] MemberDto memberDto)
        {
            var result = await memberService.CreateMember(memberDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Created("/member/10", null);
        }

    }
}
