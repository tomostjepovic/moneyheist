using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Application.Services;
using MoneyHeist.Data.Dtos.Heist;
using MoneyHeist.Data.ErrorCodes;
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

        [HttpPut]
        [Route("{id}/start")]
        public async Task<IActionResult> StartHeist(int id)
        {
            var result = await heistService.StartHeist(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == HeistErrors.HeistNotFound)
                {
                    return NotFound();
                }
                if (result.ErrorMessage == HeistErrors.HeistStatusNotReady)
                {
                    return StatusCode(StatusCodes.Status405MethodNotAllowed);
                }

                return BadRequest(result.ErrorMessage);
            }

            Response.Headers["Location"] = $"/heist/{id}/status";

            return Ok();
        }

        [HttpGet]
        [Route("{id}/status")]
        public async Task<IActionResult> GetHeistStatus(int id)
        {
            var heistStatusResult = await heistService.GetHeistStatus(id);
            if (!heistStatusResult.Success)
            {
                if (heistStatusResult.ErrorMessage == HeistErrors.HeistNotFound)
                {
                    return NotFound();
                }
                
                return BadRequest(heistStatusResult.ErrorMessage);
            }

            return Ok(heistStatusResult.StatusDto);
        }

        [HttpGet]
        [Route("{id}/eligible_members")]
        public async Task<IActionResult> GetEligibleMembers(int id)
        {
            var eligibleMembersResult = await heistService.GetHeistEligibleMembers(id);

            if (!eligibleMembersResult.Success)
            {
                if (eligibleMembersResult.ErrorMessage == HeistErrors.HeistNotFound)
                {
                    return NotFound();
                }

                if (eligibleMembersResult.ErrorMessage == HeistErrors.MembersAlreadyConfirmed)
                {
                    return StatusCode(StatusCodes.Status405MethodNotAllowed);
                }
                return BadRequest(eligibleMembersResult.ErrorMessage);
            }

            return Ok(eligibleMembersResult.EligibleMembers);
        }

        [HttpGet]
        [Route("{id}/members")]
        public async Task<IActionResult> GetMembers(int id)
        {
            
            var heistMembers = await heistService.GetHeistMembers(id);

            if (!heistMembers.Success)
            {
                if (heistMembers.ErrorMessage == HeistErrors.HeistNotFound)
                {
                    return NotFound();
                }

                if (heistMembers.ErrorMessage == HeistErrors.HeistInPlaning)
                {
                    return StatusCode(StatusCodes.Status405MethodNotAllowed);
                }
                return BadRequest(heistMembers.ErrorMessage);
            }

            return Ok(heistMembers.Members);
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
            var result = await heistService.UpdateHeistSkills(id, updateHeistSkillsDto);

            if (!result.Success)
            {
                if (result.ErrorMessage == HeistErrors.HeistNotFound)
                {
                    return NotFound();
                }
                if (result.ErrorMessage == HeistErrors.HeistHasStarted)
                {
                    return StatusCode(StatusCodes.Status405MethodNotAllowed);
                }
                return BadRequest(result);
            }

            // TODO: set URI correctly
            Response.Headers["Location"] = $"/heist/{id}/skills";

            return NoContent();
        }

        [HttpPut]
        [Route("{id}/members")]
        public async Task<IActionResult> AssignMembersToHeist([FromRoute] int id, [FromBody] AssignMembersToHeistDto assignMembersToHeistDto)
        {
            /*
            var result = await heistService.UpdateHeistSkills(id, updateHeistSkillsDto);

            if (!result.Success)
            {
                if (result.ErrorMessage == HeistErrors.HeistNotFound)
                {
                    return NotFound();
                }
                if (result.ErrorMessage == HeistErrors.HeistNotInPlaning)
                {
                    return StatusCode(StatusCodes.Status405MethodNotAllowed);
                }
                return BadRequest(result);
            }

            // TODO: set URI correctly
            Response.Headers["Location"] = $"/heist/{id}/members";
            */
            return NoContent();
        }
    }
}
