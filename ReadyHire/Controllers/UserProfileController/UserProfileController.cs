using ReadyHire.Models.Authentication;
using ReadyHire.Models.Dto;
using ReadyHire.Models.Dto.UserProfileDto;
using ReadyHire.Models.UserProfile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ReadyHire.Controllers.UserProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Get All UserProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAllUserProfiles()
        {
            var profiles = await _context.UserProfiles
                .Include(u => u.cv)
                .Include(u => u.UserOverView)
                .Include(u => u.Educations)
                .Include(u => u.Experiences)
                .Include(u => u.Skills)
                .ToListAsync();

            var result = new List<UserProfileDto>();

            foreach (var profile in profiles)
            {
                var latestPicture = await _context.userProfilePictures
                    .Where(p => p.UserId == profile.ApplicationUserId)
                    .OrderByDescending(p => p.CreatedAt)
                    .FirstOrDefaultAsync();

                var dto = new UserProfileDto
                {
                    Id = profile.Id,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Location = profile.Location,
                    JobTitle = profile.JobTitle,
                    ApplicationUserId = profile.ApplicationUserId,
                    CvId = profile.CvId,
                    Cv = profile.cv != null ? new CvDto
                    {
                        Id = profile.cv.Id,
                        CvFilePath = profile.cv.CvFilePath,
                        UserProfileId = profile.cv.UserProfileId
                    } : null,
                    UserOverViewId = profile.UserOverViewId,
                    UserOverView = profile.UserOverView != null ? new UserOverViewDto
                    {
                        Id = profile.UserOverView.Id,
                        Title = profile.UserOverView.Title,
                        Disciption = profile.UserOverView.Disciption,
                        UserProfileId = profile.UserOverView.UserProfileId
                    } : null,

                    Educations = profile.Educations.Select(e => new EducationDto
                    {
                        Id = e.Id,
                        University = e.University,
                        Faculty = e.Faculty,
                        Degree = e.Degree,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        UserProfileId = e.UserProfileId
                    }).ToList(),
                    Experiences = profile.Experiences.Select(ex => new ExperienceDto
                    {
                        Id = ex.Id,
                        JobTitle = ex.JobTitle,
                        OrganizationName = ex.OrganizationName,
                        StartDate = ex.StartDate,
                        EndDate = ex.EndDate,
                        UserProfileId = ex.UserProfileId
                    }).ToList(),
                    Skills = profile.Skills.Select(s => new SkillsDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        UserProfileId = s.UserProfileId
                    }).ToList(),
                    UserProfilePicture = latestPicture != null ? new UserProfilePictureDto
                    {
                        Id = latestPicture.UserProfilePictureId,
                        Url = latestPicture.Image,
                        UserId = latestPicture.UserId
                    } : null
                };

                result.Add(dto);
            }

            return Ok(result);
        }

        // 2️⃣ Get UserProfile by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDto>> GetById(int id)
        {
            var profile = await _context.UserProfiles
                .Include(u => u.cv)
                .Include(u => u.UserOverView)
                .Include(u => u.Educations)
                .Include(u => u.Experiences)
                .Include(u => u.Skills)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (profile == null)
                return NotFound();

            var profilePicture = await _context.userProfilePictures
                .Where(p => p.UserId == profile.ApplicationUserId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            var dto = new UserProfileDto
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Location = profile.Location,
                JobTitle = profile.JobTitle,
                ApplicationUserId = profile.ApplicationUserId,
                CvId = profile.CvId,
                Cv = profile.cv != null ? new CvDto
                {
                    Id = profile.cv.Id,
                    CvFilePath = profile.cv.CvFilePath,
                    UserProfileId = profile.cv.UserProfileId
                } : null,
                UserOverViewId = profile.UserOverViewId,
                UserOverView = profile.UserOverView != null ? new UserOverViewDto
                {
                    Id = profile.UserOverView.Id,
                    Title = profile.UserOverView.Title,
                    Disciption = profile.UserOverView.Disciption,
                    UserProfileId = profile.UserOverView.UserProfileId
                } : null,

                Educations = profile.Educations?.Select(e => new EducationDto
                {
                    Id = e.Id,
                    University = e.University,
                    Faculty = e.Faculty,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    UserProfileId = e.UserProfileId
                }).ToList() ?? new List<EducationDto>(),
                Experiences = profile.Experiences?.Select(ex => new ExperienceDto
                {
                    Id = ex.Id,
                    JobTitle = ex.JobTitle,
                    OrganizationName = ex.OrganizationName,
                    StartDate = ex.StartDate,
                    EndDate = ex.EndDate,
                    UserProfileId = ex.UserProfileId
                }).ToList() ?? new List<ExperienceDto>(),
                Skills = profile.Skills?.Select(s => new SkillsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    UserProfileId = s.UserProfileId
                }).ToList() ?? new List<SkillsDto>(),
                UserProfilePicture = profilePicture != null ? new UserProfilePictureDto
                {
                    Id = profilePicture.UserProfilePictureId,
                    Url = profilePicture.Image,
                    UserId = profilePicture.UserId
                } : null
            };

            return Ok(dto);
        }

        // 3️⃣ Get by ApplicationUserId
        [HttpGet("ByUser/{ApplicationUserId}")]
        public async Task<ActionResult<UserProfileDto>> GetByUserId(string ApplicationUserId)
        {
            var profile = await _context.UserProfiles
                .Include(u => u.cv)
                .Include(u => u.UserOverView)
                .Include(u => u.Educations)
                .Include(u => u.Experiences)
                .Include(u => u.Skills)
                .FirstOrDefaultAsync(u => u.ApplicationUserId == ApplicationUserId);

            if (profile == null)
                return NotFound();

            var dto = new UserProfileDto
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Location = profile.Location,
                JobTitle = profile.JobTitle,
                ApplicationUserId = profile.ApplicationUserId,
                CvId = profile.CvId,
                Cv = new CvDto
                {
                    Id = profile.cv.Id,
                    UserProfileId = profile.cv.UserProfileId,
                    CvFilePath = profile.cv.CvFilePath
                },
                UserOverViewId = profile.UserOverViewId,
                UserOverView = new UserOverViewDto
                {
                    Id = profile.UserOverView.Id,
                    Title = profile.UserOverView.Title,
                    Disciption = profile.UserOverView.Disciption,
                    UserProfileId = profile.UserOverView.UserProfileId
                },
                Educations = profile.Educations.Select(e => new EducationDto
                {
                    Id = e.Id,
                    University = e.University,
                    Faculty = e.Faculty,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    UserProfileId = e.UserProfileId
                }).ToList(),
                Experiences = profile.Experiences.Select(ex => new ExperienceDto
                {
                    Id = ex.Id,
                    JobTitle = ex.JobTitle,
                    OrganizationName = ex.OrganizationName,
                    StartDate = ex.StartDate,
                    EndDate = ex.EndDate,
                    UserProfileId = ex.UserProfileId
                }).ToList(),
                Skills = profile.Skills.Select(s => new SkillsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    UserProfileId = s.UserProfileId
                }).ToList()
            };

            return Ok(dto);
        }

        // 4️⃣ Create
        [HttpPost]
        public async Task<IActionResult> Create(UserProfileCreateDto dto)
        {
            var profile = new UserProfiles
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Location = dto.Location,
                JobTitle = dto.JobTitle,
                ApplicationUserId = dto.ApplicationUserId
            };

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(dto.ApplicationUserId);
            if (user != null)
            {
                user.UserProfileId = profile.Id;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            var result = new UserProfileCreateDto
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Location = profile.Location,
                JobTitle = profile.JobTitle,
                ApplicationUserId = profile.ApplicationUserId
            };

            return CreatedAtAction(nameof(GetById), new { id = profile.Id }, result);
        }

        // 5️⃣ Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] Dictionary<string, object> updates)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            if (profile == null)
                return NotFound();

            foreach (var update in updates)
            {
                var prop = typeof(UserProfiles).GetProperty(update.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(profile, Convert.ChangeType(update.Value, prop.PropertyType));
                }
            }

            _context.Entry(profile).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 6️⃣ Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            if (profile == null)
                return NotFound();

            _context.UserProfiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
