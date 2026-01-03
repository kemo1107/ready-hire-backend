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
    public class CompanyProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompanyProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Get All Company Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyProfileDto>>> GetAllCompanyProfiles()
        {
            var companies = await _context.CompanyProfiles
                .Select(c => new CompanyProfileDto
                {
                    Id = c.Id,
                    CompanyName = c.CompanyName,
                    ResponsiblePersonJobTitle = c.ResponsiblePersonJobTitle,
                    Industry = c.Industry,
                    OfficialContactMethods = c.OfficialContactMethods,
                    Location = c.Location,
                    YearEstablished = c.YearEstablished,
                    CreatedAt = c.CreatedAt,
                    ApplicationUserId = c.ApplicationUserId
                }).ToListAsync();

            return Ok(companies);
        }

        // 2️⃣ Get Company Profile By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyProfileDto>> GetById(int id)
        {
            var company = await _context.CompanyProfiles.FindAsync(id);
            if (company == null)
                return NotFound();

            // استعلام عن صورة الشركة من جدول UserProfilePictures
            var companyImageUrl = await _context.userProfilePictures
                .Where(p => p.UserId == company.ApplicationUserId)
                .Select(p => p.Image)
                .FirstOrDefaultAsync();

            var dto = new CompanyProfileDto
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                ResponsiblePersonJobTitle = company.ResponsiblePersonJobTitle,
                Industry = company.Industry,
                OfficialContactMethods = company.OfficialContactMethods,
                Location = company.Location,
                YearEstablished = company.YearEstablished,
                CreatedAt = company.CreatedAt,
                ApplicationUserId = company.ApplicationUserId,
                CompanyImageUrl = companyImageUrl // ✅ أضف رابط الصورة هنا
            };

            return Ok(dto);
        }


        // 3️⃣ Get Company Profile By ApplicationUserId
        [HttpGet("ByUser/{applicationUserId}")]
        public async Task<ActionResult<CompanyProfileDto>> GetByApplicationUserId(string applicationUserId)
        {
            var company = await _context.CompanyProfiles
                .Where(c => c.ApplicationUserId == applicationUserId)
                .FirstOrDefaultAsync();

            if (company == null)
                return NotFound();

            var dto = new CompanyProfileDto
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                ResponsiblePersonJobTitle = company.ResponsiblePersonJobTitle,
                Industry = company.Industry,
                OfficialContactMethods = company.OfficialContactMethods,
                Location = company.Location,
                YearEstablished = company.YearEstablished,
                CreatedAt = company.CreatedAt,
                ApplicationUserId = company.ApplicationUserId
            };

            return Ok(dto);
        }

        // 4️⃣ Create Company Profile
        [HttpPost]
        public async Task<IActionResult> Create(CompanyProfileDto dto)
        {
            var company = new CompanyProfiles
            {
                CompanyName = dto.CompanyName,
                ResponsiblePersonJobTitle = dto.ResponsiblePersonJobTitle,
                Industry = dto.Industry,
                OfficialContactMethods = dto.OfficialContactMethods,
                Location = dto.Location,
                YearEstablished = dto.YearEstablished,
                CreatedAt = DateTime.UtcNow,
                ApplicationUserId = dto.ApplicationUserId
            };

            _context.CompanyProfiles.Add(company);
            await _context.SaveChangesAsync();

            // Optional: Update ApplicationUser لو بتحب تربطه
            var user = await _context.Users.FindAsync(dto.ApplicationUserId);
            if (user != null)
            {
                user.CompanyProfilesId = company.Id;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            dto.Id = company.Id;
            dto.CreatedAt = company.CreatedAt;

            return CreatedAtAction(nameof(GetById), new { id = company.Id }, dto);
        }

        // 5️⃣ Patch Company Profile
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] Dictionary<string, object> updates)
        {
            var company = await _context.CompanyProfiles.FindAsync(id);
            if (company == null)
                return NotFound();

            foreach (var update in updates)
            {
                var prop = typeof(CompanyProfiles).GetProperty(update.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(company, Convert.ChangeType(update.Value, prop.PropertyType));
                }
            }

            _context.Entry(company).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 6️⃣ Delete Company Profile
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _context.CompanyProfiles.FindAsync(id);
            if (company == null)
                return NotFound();

            _context.CompanyProfiles.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}