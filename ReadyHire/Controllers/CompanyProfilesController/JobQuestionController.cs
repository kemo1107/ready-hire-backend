using ReadyHire.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ReadyHire.Controllers.CompanyProfilesController
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobQuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Get All Job Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobQuestionDto>>> GetAllJobQuestions()
        {
            var questions = await _context.JobQuestions
                .Select(q => new JobQuestionDto
                {
                    Id = q.Id,
                    JobExamId = q.JobExamId,
                    QuestionText = q.QuestionText,
                    Choices = q.Choices,
                    CorrectAnswer = q.CorrectAnswer
                }).ToListAsync();

            return Ok(questions);
        }

        // 2️⃣ Get Job Question By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<JobQuestionDto>> GetJobQuestionById(int id)
        {
            var question = await _context.JobQuestions
                .Where(q => q.Id == id)
                .Select(q => new JobQuestionDto
                {
                    Id = q.Id,
                    JobExamId = q.JobExamId,
                    QuestionText = q.QuestionText,
                    Choices = q.Choices,
                    CorrectAnswer = q.CorrectAnswer
                }).FirstOrDefaultAsync();

            if (question == null)
                return NotFound();

            return Ok(question);
        }

        // 3️⃣ Get Questions By JobExamId
        [HttpGet("ByExam/{examId}")]
        public async Task<ActionResult<IEnumerable<JobQuestionDto>>> GetQuestionsByExamId(int examId)
        {
            var questions = await _context.JobQuestions
                .Where(q => q.JobExamId == examId)
                .Select(q => new JobQuestionDto
                {
                    Id = q.Id,
                    JobExamId = q.JobExamId,
                    QuestionText = q.QuestionText,
                    Choices = q.Choices,
                    CorrectAnswer = q.CorrectAnswer
                }).ToListAsync();

            return Ok(questions);
        }

        // 4️⃣ Create a New Job Question
        [HttpPost]
        public async Task<IActionResult> Create(JobQuestionDto dto)
        {
            var question = new JobQuestion
            {
                JobExamId = dto.JobExamId,
                QuestionText = dto.QuestionText,
                Choices = dto.Choices,
                CorrectAnswer = dto.CorrectAnswer
            };

            _context.JobQuestions.Add(question);
            await _context.SaveChangesAsync();

            dto.Id = question.Id;

            return CreatedAtAction(nameof(GetJobQuestionById), new { id = question.Id }, dto);
        }

        // 5️⃣ Patch Job Question
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] Dictionary<string, object> updates)
        {
            var question = await _context.JobQuestions.FindAsync(id);
            if (question == null)
                return NotFound();

            foreach (var update in updates)
            {
                var prop = typeof(JobQuestion).GetProperty(update.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(question, Convert.ChangeType(update.Value, prop.PropertyType));
                }
            }

            _context.Entry(question).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 6️⃣ Delete Job Question
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _context.JobQuestions.FindAsync(id);
            if (question == null)
                return NotFound();

            _context.JobQuestions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}