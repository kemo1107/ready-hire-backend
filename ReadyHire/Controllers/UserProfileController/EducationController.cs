using ReadyHire.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ReadyHire.Controllers.UserProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public EducationController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ✅ 1. Get all education records (admin/test purposes)
        [HttpGet("GetEducations")]
        public async Task<ActionResult<IEnumerable<EducationDto>>> GetEducations()
        {
            var educations = await _db.Educations
                .Select(e => new EducationDto
                {
                    Id = e.Id,
                    University = e.University,
                    Faculty = e.Faculty,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    UserProfileId = e.UserProfileId
                })
                .ToListAsync();

            return Ok(educations);
        }

        // ✅ 2. Get all education records for a specific UserProfileId
        [HttpGet("by-profile/{userProfileId}")]
        public async Task<ActionResult<IEnumerable<EducationDto>>> GetEducationsByUserProfile(int userProfileId)
        {
            var educations = await _db.Educations
                .Where(e => e.UserProfileId == userProfileId)
                .Select(e => new EducationDto
                {
                    Id = e.Id,
                    University = e.University,
                    Faculty = e.Faculty,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    UserProfileId = e.UserProfileId
                })
                .ToListAsync();

            return Ok(educations);
        }

        // ✅ 3. Get a specific education record by id
        [HttpGet("{id}")]
        public async Task<ActionResult<EducationDto>> GetEducation(int id)
        {
            var education = await _db.Educations
                .Where(e => e.Id == id)
                .Select(e => new EducationDto
                {
                    Id = e.Id,
                    University = e.University,
                    Faculty = e.Faculty,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    UserProfileId = e.UserProfileId
                })
                .FirstOrDefaultAsync();

            if (education == null)
                return NotFound("Education record not found.");

            return Ok(education);
        }

        // ✅ 4. Add a new education record
        [HttpPost]
        public async Task<IActionResult> AddEducation(EducationDto dto)
        {
            var education = new Education
            {
                University = dto.University,
                Faculty = dto.Faculty,
                Degree = dto.Degree,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                UserProfileId = dto.UserProfileId
            };

            _db.Educations.Add(education);
            await _db.SaveChangesAsync();

            return Ok("Education added successfully.");
        }

        // ✅ 5. Update an education record (PATCH)
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateEducation(int id, [FromBody] EducationDto dto)
        {
            var education = await _db.Educations.FindAsync(id);
            if (education == null)
                return NotFound("Education record not found.");

            education.University = dto.University;
            education.Faculty = dto.Faculty;
            education.Degree = dto.Degree;
            education.StartDate = dto.StartDate;
            education.EndDate = dto.EndDate;

            await _db.SaveChangesAsync();
            return Ok("Education updated successfully.");
        }

        // ✅ 6. Delete an education record
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            var education = await _db.Educations.FindAsync(id);
            if (education == null)
                return NotFound("Education record not found.");

            _db.Educations.Remove(education);
            await _db.SaveChangesAsync();

            return Ok("Education deleted successfully.");
        }
    }
}

