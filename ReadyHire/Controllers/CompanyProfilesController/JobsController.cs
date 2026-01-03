using ReadyHire.Models.Authentication;
using ReadyHire.Models.CompanyProfile;
using ReadyHire.Models.Dto.CompanyProfileDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ReadyHire.Controllers.CompanyProfilesController
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Get All Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetAllJobs()
        {
            var jobs = await _context.Jobs
                .Include(j => j.CompanyProfile)
                .Select(j => new JobDto
                {
                    Id = j.Id,
                    JobTitle = j.JobTitle,
                    JobCategory = j.JobCategory,
                    JobType = j.JobType,
                    ExperienceLevel = j.ExperienceLevel,
                    Skills = j.Skills,
                    JobDescription = j.JobDescription,
                    DeadlineForApplications = j.DeadlineForApplications,
                    ExpectedSalary = j.ExpectedSalary,
                    IsSalaryNegotiable = j.IsSalaryNegotiable,
                    WorkingHours = j.WorkingHours,
                    JobLocation = j.JobLocation,
                    CreatedAt = j.CreatedAt,
                    CompanyProfileId = j.CompanyProfileId,

                    CompanyName = j.CompanyProfile.CompanyName,
                    CompanyLocation = j.CompanyProfile.Location,
                    CompanyImageUrl = _context.userProfilePictures
                        .Where(p => p.UserId == j.CompanyProfile.ApplicationUserId)
                        .Select(p => p.Image)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(jobs);
        }



        // 7️⃣ Get Job Application Summary By CompanyProfileId
        [HttpGet("SummaryByCompany/{companyProfileId}")]
        public async Task<ActionResult<IEnumerable<JobApplicationSummaryDto>>> GetJobApplicationSummaryByCompany(int companyProfileId)
        {
            var result = await _context.Jobs
                .Where(j => j.CompanyProfileId == companyProfileId)
                .Select(j => new JobApplicationSummaryDto
                {
                    JobId = j.Id,
                    JobTitle = j.JobTitle,
                    NumberOfApplicants = _context.JobApplications.Count(a => a.JobId == j.Id)
                })
                .ToListAsync();

            return Ok(result);
        }


        // 2️⃣ Get Job by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<JobDto>> GetJobById(int id)
        {
            var job = await _context.Jobs
                .Include(j => j.CompanyProfile)
                .Where(j => j.Id == id)
                .Select(j => new JobDto
                {
                    Id = j.Id,
                    JobTitle = j.JobTitle,
                    JobCategory = j.JobCategory,
                    JobType = j.JobType,
                    ExperienceLevel = j.ExperienceLevel,
                    Skills = j.Skills,
                    JobDescription = j.JobDescription,
                    DeadlineForApplications = j.DeadlineForApplications,
                    ExpectedSalary = j.ExpectedSalary,
                    IsSalaryNegotiable = j.IsSalaryNegotiable,
                    WorkingHours = j.WorkingHours,
                    JobLocation = j.JobLocation,
                    CreatedAt = j.CreatedAt,
                    CompanyProfileId = j.CompanyProfileId,

                    CompanyName = j.CompanyProfile.CompanyName,
                    CompanyLocation = j.CompanyProfile.Location,
                    CompanyImageUrl = _context.userProfilePictures
                        .Where(p => p.UserId == j.CompanyProfile.ApplicationUserId)
                        .Select(p => p.Image)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (job == null)
                return NotFound();

            return Ok(job);
        }


        // 3️⃣ Get Jobs by CompanyProfileId
        [HttpGet("ByCompany/{companyProfileId}")]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetJobsByCompanyProfileId(int companyProfileId)
        {
            var jobs = await _context.Jobs
                .Include(j => j.CompanyProfile)
                .Where(j => j.CompanyProfileId == companyProfileId)
                .Select(j => new JobDto
                {
                    Id = j.Id,
                    JobTitle = j.JobTitle,
                    JobCategory = j.JobCategory,
                    JobType = j.JobType,
                    ExperienceLevel = j.ExperienceLevel,
                    Skills = j.Skills,
                    JobDescription = j.JobDescription,
                    DeadlineForApplications = j.DeadlineForApplications,
                    ExpectedSalary = j.ExpectedSalary,
                    IsSalaryNegotiable = j.IsSalaryNegotiable,
                    WorkingHours = j.WorkingHours,
                    JobLocation = j.JobLocation,
                    CreatedAt = j.CreatedAt,
                    CompanyProfileId = j.CompanyProfileId,

                    CompanyName = j.CompanyProfile.CompanyName,
                    CompanyLocation = j.CompanyProfile.Location,
                    CompanyImageUrl = _context.userProfilePictures
                        .Where(p => p.UserId == j.CompanyProfile.ApplicationUserId)
                        .Select(p => p.Image)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(jobs);
        }


        // 8️⃣ Get Job Titles by CompanyProfileId
        [HttpGet("TitlesByCompany/{companyProfileId}")]
        public async Task<ActionResult<IEnumerable<JobTitleDto>>> GetJobTitlesByCompany(int companyProfileId)
        {
            var jobTitles = await _context.Jobs
                .Where(j => j.CompanyProfileId == companyProfileId)
                .Select(j => new JobTitleDto
                {
                    JobId = j.Id,
                    JobTitle = j.JobTitle
                })
                .ToListAsync();

            return Ok(jobTitles);
        }


        // 4️⃣ Create a New Job
        [HttpPost]
        public async Task<IActionResult> Create(JobDto dto)
        {
            var job = new Job
            {
                JobTitle = dto.JobTitle,
                JobCategory = dto.JobCategory,
                JobType = dto.JobType,
                ExperienceLevel = dto.ExperienceLevel,
                Skills = dto.Skills,
                JobDescription = dto.JobDescription,
                DeadlineForApplications = dto.DeadlineForApplications,
                ExpectedSalary = dto.ExpectedSalary,
                IsSalaryNegotiable = dto.IsSalaryNegotiable,
                WorkingHours = dto.WorkingHours,
                JobLocation = dto.JobLocation,
                CreatedAt = DateTime.UtcNow,
                CompanyProfileId = dto.CompanyProfileId
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            dto.Id = job.Id;
            dto.CreatedAt = job.CreatedAt;

            return CreatedAtAction(nameof(GetJobById), new { id = job.Id }, dto);
        }

        // 5️⃣ Patch a Job
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] Dictionary<string, object> updates)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound();

            foreach (var update in updates)
            {
                var prop = typeof(Job).GetProperty(update.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(job, Convert.ChangeType(update.Value, prop.PropertyType));
                }
            }

            _context.Entry(job).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 6️⃣ Delete a Job
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound();

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
