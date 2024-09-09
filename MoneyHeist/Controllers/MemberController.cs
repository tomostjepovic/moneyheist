using Microsoft.AspNetCore.Mvc;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.Data.ErrorCodes;
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
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await memberService.GetMemberById(id);
            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        [HttpGet]
        [Route("{id}/skills")]
        public async Task<IActionResult> GetMemberSkills(int id)
        {
            var member = await memberService.GetMemberById(id);
            if (member == null)
            {
                return NotFound();
            }

            return Ok(member.Skills);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateMember([FromBody] MemberDto memberDto)
        {
            var result = await memberService.CreateMember(memberDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Created($"/member/{result.MemberID}", null);
        }

        [HttpPut]
        [Route("{id}/skills")]
        public async Task<IActionResult> UpdateMemberSkills([FromRoute] int id, [FromBody] MemberSkillsDto updateMemberSkillsDto)
        {
            var result = await memberService.UpdateMemberSkills(id, updateMemberSkillsDto);

            if (!result.Success)
            {
                if (result.ErrorMessage == HeistErrors.MemberNotFound)
                {
                    return NotFound();
                }
                return BadRequest(result);
            }

            Response.Headers["Content-Location"] = $"/member/{id}/skills";

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}/skills/{skillName}")]
        public async Task<IActionResult> DeleteMemberSkills([FromRoute] int id, [FromRoute] string skillName)
        {
            var result = await memberService.DeleteMemberSkills(id, skillName);

            if (!result.Success)
            {
                if (result.ErrorMessage != HeistErrors.MemberOrMemberSkillNotFound)
                {
                    return NotFound();
                }
                return BadRequest(result);
            }

            return NoContent();
        }

    }
}
