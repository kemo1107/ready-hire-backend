using ReadyHire.Models.Authentication;
using ReadyHire.Models.Dto;
using ReadyHire.Models.UserProfilePic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ReadyHire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilePicController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UserProfilePicController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ✅ Add new profile picture
        [HttpPost("AddUserProfilePicture")]
        public async Task<IActionResult> AddUserProfilePicture([FromForm] UserProfilePictureAddDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Image == null || dto.Image.Length == 0)
                return BadRequest("Image is required.");

            var fileName = $"{Guid.NewGuid()}_{dto.Image.FileName}";
            var savePath = Path.Combine(_environment.WebRootPath, "user-profile-pics");
            Directory.CreateDirectory(savePath);
            var filePath = Path.Combine(savePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            var relativePath = Path.Combine("user-profile-pics", fileName).Replace("\\", "/");
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var fullUrl = $"{baseUrl}/{relativePath}";

            var userProfilePicture = new UserProfilePic
            {
                UserId = dto.UserId,
                Image = fullUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.userProfilePictures.Add(userProfilePicture);
            await _context.SaveChangesAsync();

            return Ok(new UserProfilePictureDto
            {
                Id = userProfilePicture.UserProfilePictureId,
                Url = userProfilePicture.Image,
                UserId = userProfilePicture.UserId
            });
        }

        // ✅ Get picture by ID
        [HttpGet("GetUserProfilePictureById/{id}")]
        public async Task<IActionResult> GetUserProfilePictureById(int id)
        {
            var userProfilePicture = await _context.userProfilePictures.FindAsync(id);
            if (userProfilePicture == null)
                return NotFound("User Profile Picture Not Found.");

            return Ok(new UserProfilePictureDto
            {
                Id = userProfilePicture.UserProfilePictureId,
                Url = userProfilePicture.Image,
                UserId = userProfilePicture.UserId
            });
        }

        // ✅ Get picture by UserId
        [HttpGet("GetUserProfilePictureByUserId/{userId}")]
        public async Task<IActionResult> GetUserProfilePictureByUserId(string userId)
        {
            var userProfilePicture = await _context.userProfilePictures
                .FirstOrDefaultAsync(up => up.UserId == userId);

            if (userProfilePicture == null)
                return NotFound("User Profile Picture Not Found.");

            return Ok(new UserProfilePictureDto
            {
                Id = userProfilePicture.UserProfilePictureId,
                Url = userProfilePicture.Image,
                UserId = userProfilePicture.UserId
            });
        }

        // ✅ Edit profile picture
        [HttpPut("EditUserProfilePicture/{id}")]
        public async Task<IActionResult> EditUserProfilePicture(int id, [FromForm] UserProfilePictureEditDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userProfilePicture = await _context.userProfilePictures.FindAsync(id);
            if (userProfilePicture == null)
                return NotFound("User Profile Picture Not Found.");

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{dto.Image.FileName}";
                var savePath = Path.Combine(_environment.WebRootPath, "user-profile-pics");
                Directory.CreateDirectory(savePath);
                var filePath = Path.Combine(savePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                var relativePath = Path.Combine("user-profile-pics", fileName).Replace("\\", "/");
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var fullUrl = $"{baseUrl}/{relativePath}";

                userProfilePicture.Image = fullUrl;
                userProfilePicture.UpdatedAt = DateTime.UtcNow;
            }

            _context.userProfilePictures.Update(userProfilePicture);
            await _context.SaveChangesAsync();

            return Ok(new UserProfilePictureDto
            {
                Id = userProfilePicture.UserProfilePictureId,
                Url = userProfilePicture.Image,
                UserId = userProfilePicture.UserId
            });
        }

        // ✅ Delete profile picture
        [HttpDelete("DeleteUserProfilePicture/{id}")]
        public async Task<IActionResult> DeleteUserProfilePicture(int id)
        {
            var userProfilePicture = await _context.userProfilePictures.FindAsync(id);
            if (userProfilePicture == null)
                return NotFound("User Profile Picture Not Found.");

            _context.userProfilePictures.Remove(userProfilePicture);
            await _context.SaveChangesAsync();

            return Ok("User Profile Picture Deleted.");
        }
    }
}
