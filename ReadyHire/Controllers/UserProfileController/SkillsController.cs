using ReadyHire.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ReadyHire.Controllers.UserProfileController.UserProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ 1. Get all skills (admin/test purposes)
        [HttpGet("GetSkills")]
        public async Task<ActionResult<IEnumerable<SkillsDto>>> GetSkills()
        {
            var skills = await _context.Skills
                .Select(s => new SkillsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    UserProfileId = s.UserProfileId
                })
                .ToListAsync();

            return Ok(skills);
        }

        // ✅ 2. Get skills by UserProfileId
        [HttpGet("by-profile/{userProfileId}")]
        public async Task<ActionResult<IEnumerable<SkillsDto>>> GetSkillsByUserProfile(int userProfileId)
        {
            var skills = await _context.Skills
                .Where(s => s.UserProfileId == userProfileId)
                .Select(s => new SkillsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    UserProfileId = s.UserProfileId
                })
                .ToListAsync();

            return Ok(skills);
        }

        // ✅ 3. Add new skill
        [HttpPost]
        public async Task<IActionResult> AddSkill([FromBody] SkillsDto dto)
        {
            var skill = new Skills
            {
                Name = dto.Name,
                UserProfileId = dto.UserProfileId
            };

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return Ok("Skill added successfully.");
        }

        // ✅ 4. Update skill (PATCH instead of PUT)
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] SkillsDto dto)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound("Skill not found.");

            skill.Name = dto.Name;

            await _context.SaveChangesAsync();
            return Ok("Skill updated successfully.");
        }

        // ✅ 5. Delete skill
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound("Skill not found.");

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();

            return Ok("Skill deleted successfully.");
        }
    }
}
