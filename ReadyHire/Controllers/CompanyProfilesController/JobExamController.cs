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
    
     public class JobExamsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobExamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Get All Job Exams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobExamDto>>> GetAllJobExams()
        {
            var exams = await _context.JobExams
                .Select(e => new JobExamDto
                {
                    Id = e.Id,
                    JobId = e.JobId,
                    Title = e.Title
                }).ToListAsync();

            return Ok(exams);
        }

        // 2️⃣ Get Job Exam By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<JobExamDto>> GetJobExamById(int id)
        {
            var exam = await _context.JobExams
                .Where(e => e.Id == id)
                .Select(e => new JobExamDto
                {
                    Id = e.Id,
                    JobId = e.JobId,
                    Title = e.Title
                }).FirstOrDefaultAsync();

            if (exam == null)
                return NotFound();

            return Ok(exam);
        }

        // 3️⃣ Get Job Exam By JobId
        [HttpGet("ByJob/{jobId}")]
        public async Task<ActionResult<JobExamDto>> GetJobExamByJobId(int jobId)
        {
            var exam = await _context.JobExams
                .Where(e => e.JobId == jobId)
                .Select(e => new JobExamDto
                {
                    Id = e.Id,
                    JobId = e.JobId,
                    Title = e.Title
                }).FirstOrDefaultAsync();

            if (exam == null)
                return NotFound();

            return Ok(exam);
        }

        // 4️⃣ Create a Job Exam
        [HttpPost]
        public async Task<IActionResult> Create(JobExamDto dto)
        {
            var exam = new JobExam
            {
                JobId = dto.JobId,
                Title = dto.Title
            };

            _context.JobExams.Add(exam);
            await _context.SaveChangesAsync();

            dto.Id = exam.Id;

            return CreatedAtAction(nameof(GetJobExamById), new { id = exam.Id }, dto);
        }

        // 5️⃣ Patch Job Exam
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] Dictionary<string, object> updates)
        {
            var exam = await _context.JobExams.FindAsync(id);
            if (exam == null)
                return NotFound();

            foreach (var update in updates)
            {
                var prop = typeof(JobExam).GetProperty(update.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(exam, Convert.ChangeType(update.Value, prop.PropertyType));
                }
            }

            _context.Entry(exam).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 6️⃣ Delete Job Exam
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _context.JobExams.FindAsync(id);
            if (exam == null)
                return NotFound();

            _context.JobExams.Remove(exam);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
