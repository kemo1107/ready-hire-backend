using ReadyHire.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ReadyHire.Controllers.CompanyProfilesController
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobExamSubmissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobExamSubmissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Get All Job Exam Submissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobExamSubmissionDto>>> GetAllJobExamSubmissions()
        {
            var submissions = await _context.JobExamSubmissions
                .Select(s => new JobExamSubmissionDto
                {
                    Id = s.Id,
                    JobExamId = s.JobExamId,
                    UserProfileId = s.UserProfileId,
                    SubmittedAt = s.SubmittedAt
                }).ToListAsync();

            return Ok(submissions);
        }

        // 2️⃣ Get Job Exam Submission By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<JobExamSubmissionDto>> GetJobExamSubmissionById(int id)
        {
            var submission = await _context.JobExamSubmissions
                .Where(s => s.Id == id)
                .Select(s => new JobExamSubmissionDto
                {
                    Id = s.Id,
                    JobExamId = s.JobExamId,
                    UserProfileId = s.UserProfileId,
                    SubmittedAt = s.SubmittedAt
                }).FirstOrDefaultAsync();

            if (submission == null)
                return NotFound();

            return Ok(submission);
        }

        // 3️⃣ Get Submissions By JobExamId
        [HttpGet("ByExam/{jobExamId}")]
        public async Task<ActionResult<IEnumerable<JobExamSubmissionDto>>> GetSubmissionsByExamId(int jobExamId)
        {
            var submissions = await _context.JobExamSubmissions
                .Where(s => s.JobExamId == jobExamId)
                .Select(s => new JobExamSubmissionDto
                {
                    Id = s.Id,
                    JobExamId = s.JobExamId,
                    UserProfileId = s.UserProfileId,
                    SubmittedAt = s.SubmittedAt
                }).ToListAsync();

            return Ok(submissions);
        }

        // 4️⃣ Create a New Job Exam Submission (with Answers)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobExamSubmissionCreateRequest request)
        {
            var submission = new JobExamSubmission
            {
                JobExamId = request.Submission.JobExamId,
                UserProfileId = request.Submission.UserProfileId,
                SubmittedAt = DateTime.UtcNow
            };

            _context.JobExamSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            foreach (var answerDto in request.Answers)
            {
                var answer = new JobExamAnswer
                {
                    JobExamSubmissionId = submission.Id,
                    QuestionId = answerDto.QuestionId,
                    SelectedAnswer = answerDto.SelectedAnswer
                };
                _context.JobExamAnswers.Add(answer);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJobExamSubmissionById), new { id = submission.Id }, request);
        }

        // 5️⃣ Patch Job Exam Submission
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] Dictionary<string, object> updates)
        {
            var submission = await _context.JobExamSubmissions.FindAsync(id);
            if (submission == null)
                return NotFound();

            foreach (var update in updates)
            {
                var prop = typeof(JobExamSubmission).GetProperty(update.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(submission, Convert.ChangeType(update.Value, prop.PropertyType));
                }
            }

            _context.Entry(submission).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 6️⃣ Delete Job Exam Submission (and its Answers)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var submission = await _context.JobExamSubmissions
                .Include(s => s.Answers)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (submission == null)
                return NotFound();

            if (submission.Answers != null && submission.Answers.Any())
                _context.JobExamAnswers.RemoveRange(submission.Answers);

            _context.JobExamSubmissions.Remove(submission);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}