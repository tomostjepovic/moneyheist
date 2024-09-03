﻿using Microsoft.AspNetCore.Mvc;
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

            // TODO: set URI correctly
            return Created($"/member/{result.MemberID}", null);
        }

        [HttpPut]
        [Route("{id}/skills")]
        public async Task<IActionResult> UpdateMemberSkills([FromRoute] int id, [FromBody] MemberSkillsDto updateMemberSkillsDto)
        {
            var member = await repoContext.Members.SingleOrDefaultAsync(x => x.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            var result = await memberService.UpdateMemberSkills(id, updateMemberSkillsDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            // TODO: set URI correctly
            Response.Headers["Content-Location"] = $"/member/{id}/skills";

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}/skills/{skillName}")]
        public async Task<IActionResult> DeleteMemberSkills([FromRoute] int id, [FromRoute] string skillName)
        {
            var skill = await repoContext.MemberToSkills.SingleOrDefaultAsync(x => x.MemberID == id && x.Skill.Name.ToLower() == skillName.ToLower());
            if (skill == null)
            {
                return NotFound();
            }

            var result = await memberService.DeleteMemberSkills(id, skillName);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

    }
}
