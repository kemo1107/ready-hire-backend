using ReadyHire.Models.Authentication;
using ReadyHire.Models.Dto.CompanyProfileDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ReadyHire.Controllers.CompanyProfilesController
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Get All Job Applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllJobApplications()
        {
            var applications = await _context.JobApplications
                .Select(a => new JobApplicationDto
                {
                    Id = a.Id,
                    JobId = a.JobId,
                    UserProfileId = a.UserProfileId,
                    HasPassedExam = a.HasPassedExam,
                    AppliedAt = a.AppliedAt
                }).ToListAsync();

            return Ok(applications);
        }

        // 2️⃣ Get Job Application by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplicationDto>> GetJobApplicationById(int id)
        {
            var application = await _context.JobApplications
                .Where(a => a.Id == id)
                .Select(a => new JobApplicationDto
                {
                    Id = a.Id,
                    JobId = a.JobId,
                    UserProfileId = a.UserProfileId,
                    HasPassedExam = a.HasPassedExam,
                    AppliedAt = a.AppliedAt
                }).FirstOrDefaultAsync();

            if (application == null)
                return NotFound();

            return Ok(application);
        }

        // 3️⃣ Get Applications by JobId
        [HttpGet("ByJob/{jobId}")]
        public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetApplicationsByJobId(int jobId)
        {
            var applications = await _context.JobApplications
                .Where(a => a.JobId == jobId)
                .Select(a => new JobApplicationDto
                {
                    Id = a.Id,
                    JobId = a.JobId,
                    UserProfileId = a.UserProfileId,
                    HasPassedExam = a.HasPassedExam,
                    AppliedAt = a.AppliedAt
                }).ToListAsync();

            return Ok(applications);
        }

        // 4️⃣ Create a New Job Application
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create(JobApplicationDto dto)
        {
            var application = new JobApplication
            {
                JobId = dto.JobId,
                UserProfileId = dto.UserProfileId,
                HasPassedExam = dto.HasPassedExam,
                AppliedAt = DateTime.UtcNow,
                MatchRatio = dto.MatchRatio // ✅ أضف السطر ده
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();

            dto.Id = application.Id;
            dto.AppliedAt = application.AppliedAt;

            return CreatedAtAction(nameof(GetJobApplicationById), new { id = application.Id }, dto);
        }

        [HttpGet("ByUserProfile/{userProfileId}")]
        public async Task<ActionResult<IEnumerable<UserAppliedJobDto>>> GetJobsAppliedByUser(int userProfileId)
        {
            var jobs = await _context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.UserProfileId == userProfileId)
                .Select(a => new UserAppliedJobDto
                {
                    JobTitle = a.Job.JobTitle,       // ✅ تم التعديل
                    Type = a.Job.JobType,            // ✅ تم التعديل
                    AppliedAt = a.AppliedAt,
                    Status = string.IsNullOrWhiteSpace(a.ApplicationStatus) ? "Under Review" : a.ApplicationStatus
                })
                .ToListAsync();

            return Ok(jobs);
        }


        [HttpPatch("UpdateStatus/{jobApplicationId}")]
        public async Task<IActionResult> UpdateStatus(int jobApplicationId, [FromBody] string newStatus)
        {
            var application = await _context.JobApplications.FindAsync(jobApplicationId);
            if (application == null)
                return NotFound();

            application.ApplicationStatus = newStatus;
            _context.JobApplications.Update(application);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Status updated successfully." });
        }



        [HttpGet("PreviewApplicantsByJob/{jobId}")]
        public async Task<ActionResult<IEnumerable<JobApplicantPreviewDto>>> PreviewApplicantsByJob(int jobId)
        {
            var applicants = await _context.JobApplications
                .Where(j => j.JobId == jobId)
                .Include(j => j.UserProfile)
                    .ThenInclude(up => up.cv)
                .Include(j => j.UserProfile)
                    .ThenInclude(up => up.ApplicationUser)
                .Select(j => new JobApplicantPreviewDto
                {
                    UserProfileId = j.UserProfileId,
                    FullName = j.UserProfile.FirstName + " " + j.UserProfile.LastName,
                    MatchRatio = j.MatchRatio,
                    CvFilePath = j.UserProfile.cv.CvFilePath,
                    AppliedAt = j.AppliedAt,
                    ProfilePictureUrl = _context.userProfilePictures
                        .Where(p => p.UserId == j.UserProfile.ApplicationUser.Id)
                        .Select(p => p.Image)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(applicants);
        }



        // 5️⃣ Patch Job Application
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] Dictionary<string, object> updates)
        {
            var application = await _context.JobApplications.FindAsync(id);
            if (application == null)
                return NotFound();

            foreach (var update in updates)
            {
                var prop = typeof(JobApplication).GetProperty(update.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(application, Convert.ChangeType(update.Value, prop.PropertyType));
                }
            }

            _context.Entry(application).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("CountByJob/{jobId}")]
        public async Task<ActionResult<JobApplicantsCountDto>> GetNumberOfApplicants(int jobId)
        {
            var count = await _context.JobApplications.CountAsync(j => j.JobId == jobId);

            var dto = new JobApplicantsCountDto
            {
                JobId = jobId,
                NumberOfApplicants = count
            };

            return Ok(dto);
        }


        // 6️⃣ Delete Job Application
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var application = await _context.JobApplications.FindAsync(id);
            if (application == null)
                return NotFound();

            _context.JobApplications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
