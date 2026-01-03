using ReadyHire.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ReadyHire.Controllers.UserProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOverViewController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UserOverViewController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ✅ 1. Get all overviews
        [HttpGet("GetOverviews")]
        public async Task<ActionResult<IEnumerable<UserOverViewDto>>> GetOverviews()
        {
            var overviews = await _db.UserOverViews
                .Select(o => new UserOverViewDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    Disciption = o.Disciption,
                    UserProfileId = o.UserProfileId
                })
                .ToListAsync();

            return Ok(overviews);
        }

        // ✅ 2. Get by profile ID
        [HttpGet("by-profile/{userProfileId}")]
        public async Task<ActionResult<UserOverViewDto>> GetOverviewByProfile(int userProfileId)
        {
            var overview = await _db.UserOverViews
                .Where(o => o.UserProfileId == userProfileId)
                .Select(o => new UserOverViewDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    Disciption = o.Disciption,
                    UserProfileId = o.UserProfileId
                })
                .FirstOrDefaultAsync();

            if (overview == null)
                return NotFound("Overview not found for this profile.");

            return Ok(overview);
        }

        // ✅ 3. Get specific overview by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserOverViewDto>> GetOverview(int id)
        {
            var overview = await _db.UserOverViews
                .Where(o => o.Id == id)
                .Select(o => new UserOverViewDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    Disciption = o.Disciption,
                    UserProfileId = o.UserProfileId
                })
                .FirstOrDefaultAsync();

            if (overview == null)
                return NotFound("Overview not found.");

            return Ok(overview);
        }

        // ✅ 4. Add new overview
        [HttpPost]
        public async Task<IActionResult> AddOverview(UserOverViewDto dto)
        {
            var overview = new UserOverView
            {
                Title = dto.Title,
                Disciption = dto.Disciption,
                UserProfileId = dto.UserProfileId
            };

            _db.UserOverViews.Add(overview);
            await _db.SaveChangesAsync();

            return Ok("Overview added successfully.");
        }

        // ✅ 5. Update overview
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateOverview(int id, [FromBody] UserOverViewDto dto)
        {
            var overview = await _db.UserOverViews.FindAsync(id);
            if (overview == null)
                return NotFound("Overview not found.");

            overview.Title = dto.Title;
            overview.Disciption = dto.Disciption;

            await _db.SaveChangesAsync();
            return Ok("Overview updated successfully.");
        }

        // ✅ 6. Delete overview
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOverview(int id)
        {
            var overview = await _db.UserOverViews.FindAsync(id);
            if (overview == null)
                return NotFound("Overview not found.");

            _db.UserOverViews.Remove(overview);
            await _db.SaveChangesAsync();

            return Ok("Overview deleted successfully.");
        }
    }
}
