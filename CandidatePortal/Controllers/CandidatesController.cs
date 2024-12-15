using CandidatePortal.Models;
using CandidatePortal.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using CandidatePortal.Dtos;

namespace CandidatePortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CandidatesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] CandidateCreateDto dto)
        {
            // Validate Department enum
            if (!Enum.IsDefined(typeof(Department), dto.Department))
            {
                return BadRequest("Invalid Department.");
            }

            // Validate file
            if (dto.Resume == null || dto.Resume.Length == 0)
            {
                return BadRequest("Resume file is required.");
            }
            if (dto.Resume.Length > 5 * 1024 * 1024) // 5 MB
            {
                return BadRequest("File size exceeds the 5MB limit.");
            }
            if (!new[] { ".pdf", ".docx" }.Contains(Path.GetExtension(dto.Resume.FileName).ToLower()))
            {
                return BadRequest("Only PDF and DOCX files are allowed.");
            }

            // Save file to uploads directory
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder); // Ensure directory exists
            var filePath = Path.Combine(uploadsFolder, $"{Guid.NewGuid()}{Path.GetExtension(dto.Resume.FileName)}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Resume.CopyToAsync(stream);
            }

            // Create Candidate entity
            var candidate = new Candidate
            {
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                YearsOfExperience = dto.YearsOfExperience,
                Department = dto.Department,
                ResumePath = filePath
            };

            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Candidate registered successfully.", candidate.Id });
        }
    }
}
