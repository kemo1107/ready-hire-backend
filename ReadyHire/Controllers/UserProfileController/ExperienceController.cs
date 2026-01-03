using ReadyHire.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ReadyHire.Controllers.UserProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ExperienceController(ApplicationDbContext db)
        {
            _db = db;
        }

        // جلب كل الخبرات (لأغراض الأدمين أو التست مثلاً)
        [HttpGet("GetExperiences")]
        public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetExperiences()
        {
            var experiences = await _db.Experiences
                .Select(exp => new ExperienceDto
                {
                    Id = exp.Id,
                    JobTitle = exp.JobTitle,
                    OrganizationName = exp.OrganizationName,
                    StartDate = exp.StartDate,
                    EndDate = exp.EndDate,
                    UserProfileId = exp.UserProfileId
                })
                .ToListAsync();

            return Ok(experiences);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ExperienceDto>> GetExperienceById(int id)
        {
            var experience = await _db.Experiences
                .Where(e => e.Id == id)
                .Select(e => new ExperienceDto
                {
                    Id = e.Id,
                    JobTitle = e.JobTitle,
                    OrganizationName = e.OrganizationName,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    UserProfileId = e.UserProfileId
                })
                .FirstOrDefaultAsync();

            if (experience == null)
                return NotFound("Experience not found.");

            return Ok(experience);
        }
        // جلب كل خبرات مستخدم معين حسب UserProfileId
        [HttpGet("by-profile/{userProfileId}")]
        public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetExperiencesByUserProfile(int userProfileId)
        {
            var experiences = await _db.Experiences
                .Where(e => e.UserProfileId == userProfileId)
                .Select(e => new ExperienceDto
                {
                    Id = e.Id,
                    JobTitle = e.JobTitle,
                    OrganizationName = e.OrganizationName,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    UserProfileId = e.UserProfileId
                })
                .ToListAsync();

            return Ok(experiences);
        }

        // إضافة خبرة جديدة
        [HttpPost]
        public async Task<IActionResult> AddExperience([FromBody] ExperienceDto dto)
        {
            var experience = new Experience
            {
                JobTitle = dto.JobTitle,
                OrganizationName = dto.OrganizationName,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                UserProfileId = dto.UserProfileId
            };

            _db.Experiences.Add(experience);
            await _db.SaveChangesAsync();

            return Ok("Experience added successfully.");
        }

        // تعديل خبرة (PATCH بدل PUT)
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateExperience(int id, [FromBody] ExperienceDto dto)
        {
            var experience = await _db.Experiences.FindAsync(id);
            if (experience == null)
                return NotFound("Experience not found.");

            experience.JobTitle = dto.JobTitle;
            experience.OrganizationName = dto.OrganizationName;
            experience.StartDate = dto.StartDate;
            experience.EndDate = dto.EndDate;

            await _db.SaveChangesAsync();
            return Ok("Experience updated successfully.");
        }

        // حذف خبرة
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var experience = await _db.Experiences.FindAsync(id);
            if (experience == null)
                return NotFound("Experience not found.");

            _db.Experiences.Remove(experience);
            await _db.SaveChangesAsync();
            return Ok("Experience deleted successfully.");
        }
    }
}
