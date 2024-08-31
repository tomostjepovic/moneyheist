using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.DataAccess;

namespace MoneyHeist.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemberController : ControllerBase
    {
        protected readonly IMemberService memberService;
        protected readonly RepoContext repoContext;

        public MemberController(IMemberService _memberService, RepoContext _repoContext)
        {
            memberService = _memberService;
            repoContext = _repoContext;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var member = await repoContext.Members.SingleOrDefaultAsync(x => x.ID == id);

            return Ok(member);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] MemberDto memberDto)
        {
            var result = await memberService.CreateMember(memberDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Created($"/member/{result.Data.ID}", null);
        }

    }
}
