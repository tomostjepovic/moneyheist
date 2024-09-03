using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Application.Services;
using MoneyHeist.Data.Dtos.Heist;
using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.DataAccess;

namespace MoneyHeist.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeistController : ControllerBase
    {
        protected readonly IHeistService heistService;
        protected readonly RepoContext repoContext;

        public HeistController(IHeistService _heistService, RepoContext _repoContext)
        {
            heistService = _heistService;
            repoContext = _repoContext;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await repoContext.Members.SingleOrDefaultAsync(x => x.ID == id);

            return Ok(member);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateHeist([FromBody] HeistDto heistDto)
        {
            var result = await heistService.CreateHeist(heistDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            // TODO: set URI correctly
            return Created($"/heist/{result.HeistID}", null);
        }

        [HttpPatch]
        [Route("{id}/skills")]
        public async Task<IActionResult> UpdateMemberSkills([FromRoute] int id, [FromBody] HeistSkillsDto updateHeistSkillsDto)
        {
            var heist = await repoContext.Heists.SingleOrDefaultAsync(x => x.ID == id);
            if (heist == null)
            {
                return NotFound();
            }

            var heistHasStarted = heistService.HeistHasStarted(heist);
            if (heistHasStarted)
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed);
            }

            var result = await heistService.UpdateHeistSkills(id, updateHeistSkillsDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            // TODO: set URI correctly
            Response.Headers["Content-Location"] = $"/heist/{id}/skills";

            return NoContent();
        }
    }
}
