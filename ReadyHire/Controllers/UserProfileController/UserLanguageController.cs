using ReadyHire.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ReadyHire.Controllers.UserProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLanguageController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UserLanguageController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ✅ 1. Get all languages (لأغراض إدارية أو اختبار)
        [HttpGet("GetLanguages")]
        public async Task<ActionResult<IEnumerable<UserLanguageDto>>> GetLanguages()
        {
            var langs = await _db.UserLanguages
                .Select(l => new UserLanguageDto
                {
                    Id = l.Id,
                    LanguageType = l.LanguageType,
                    UserProfileId = l.UserProfileId
                })
                .ToListAsync();

            return Ok(langs);
        }

        // ✅ 2. Get languages by UserProfileId
        [HttpGet("by-profile/{userProfileId}")]
        public async Task<ActionResult<IEnumerable<UserLanguageDto>>> GetLanguagesByUserProfile(int userProfileId)
        {
            var langs = await _db.UserLanguages
                .Where(l => l.UserProfileId == userProfileId)
                .Select(l => new UserLanguageDto
                {
                    Id = l.Id,
                    LanguageType = l.LanguageType,
                    UserProfileId = l.UserProfileId
                })
                .ToListAsync();

            return Ok(langs);
        }

        // ✅ 3. Get specific language by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserLanguageDto>> GetLanguage(int id)
        {
            var lang = await _db.UserLanguages
                .Where(l => l.Id == id)
                .Select(l => new UserLanguageDto
                {
                    Id = l.Id,
                    LanguageType = l.LanguageType,
                    UserProfileId = l.UserProfileId
                })
                .FirstOrDefaultAsync();

            if (lang == null)
                return NotFound("Language record not found.");

            return Ok(lang);
        }

        // ✅ 4. Add a new language
        [HttpPost]
        public async Task<IActionResult> AddLanguage(UserLanguageDto dto)
        {
            var lang = new UserLanguage
            {
                LanguageType = dto.LanguageType,
                UserProfileId = dto.UserProfileId
            };

            _db.UserLanguages.Add(lang);
            await  _db.SaveChangesAsync();

            return Ok("Language added successfully.");
        }

        // ✅ 5. Update a language (PATCH)
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateLanguage(int id, [FromBody] UserLanguageDto dto)
        {
            var lang = await _db.UserLanguages.FindAsync(id);
            if (lang == null)
                return NotFound("Language record not found.");

            lang.LanguageType = dto.LanguageType;

            await _db.SaveChangesAsync();
            return Ok("Language updated successfully.");
        }

        // ✅ 6. Delete a language
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            var lang = await _db.UserLanguages.FindAsync(id);
            if (lang == null)
                return NotFound("Language record not found.");

            _db.UserLanguages.Remove(lang);
            await _db.SaveChangesAsync();

            return Ok("Language deleted successfully.");
        }
    }
}