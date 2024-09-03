using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Application.Services;
using MoneyHeist.Data.Dtos.Heist;
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
        public async Task<IActionResult> GetHeist(int id)
        {
            var heist = await heistService.GetHeistById(id);
            if (heist == null)
            {
                return NotFound();
            }

            return Ok(heist);
        }

        [HttpGet]
        [Route("{id}/skills")]
        public async Task<IActionResult> GetHeistSkills(int id)
        {
            var heist = await heistService.GetHeistById(id);
            if (heist == null)
            {
                return NotFound();
            }

            return Ok(heist.Skills);
        }

        // TODO: implement when status added to heist
        [HttpGet]
        [Route("{id}/status")]
        public async Task<IActionResult> GetHeistStatus(int id)
        {
            var heist = await heistService.GetHeistById(id);
            if (heist == null)
            {
                return NotFound();
            }

            return Ok(new
            { });
        }

        [HttpGet]
        [Route("{id}/eligible_members")]
        public async Task<IActionResult> GetEligibleMembers(int id)
        {
            var heist = await heistService.GetHeistById(id);
            if (heist == null)
            {
                return NotFound();
            }

            var heistSkills = await heistService.GetHeistSkills(id);

            var result = new HeistEligibleMembersDto
            {
                Skills = heistSkills,
            };
            //var member = await repoContext.Members.SingleOrDefaultAsync(x => x.ID == id);

            return Ok(result);
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
