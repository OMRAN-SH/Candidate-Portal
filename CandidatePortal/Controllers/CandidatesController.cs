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


        [HttpGet("list")]
        public IActionResult ListCandidates([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Check for admin access
            if (!Request.Headers.TryGetValue("X-ADMIN", out var isAdmin) || isAdmin != "1")
            {
                return Unauthorized(new { message = "Admin access required." });
            }

            // Get candidates from the database
            var candidates = _context.Candidates
                .OrderByDescending(c => c.Id) // Assuming Id indicates registration order
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new
                {
                    c.Id,
                    c.FullName,
                    c.DateOfBirth,
                    c.YearsOfExperience,
                    Department = c.Department.ToString()
                })
                .ToList();

            return Ok(new
            {
                pageNumber,
                pageSize,
                totalCandidates = _context.Candidates.Count(),
                candidates
            });
        }

        [HttpGet("download-resume/{id}")]
        public IActionResult DownloadResume(int id)
        {
            // Check for admin access
            if (!Request.Headers.TryGetValue("X-ADMIN", out var isAdmin) || isAdmin != "1")
            {
                return Unauthorized(new { message = "Admin access required." });
            }

            // Find the candidate by ID
            var candidate = _context.Candidates.FirstOrDefault(c => c.Id == id);
            if (candidate == null)
            {
                return NotFound(new { message = "Candidate not found." });
            }

            // Verify if the resume file exists
            if (string.IsNullOrEmpty(candidate.ResumePath) || !System.IO.File.Exists(candidate.ResumePath))
            {
                return NotFound(new { message = "Resume file not found." });
            }

            // Return the file as a download
            var fileBytes = System.IO.File.ReadAllBytes(candidate.ResumePath);
            var fileName = Path.GetFileName(candidate.ResumePath);
            var contentType = "application/octet-stream"; // Default content type
            if (Path.GetExtension(candidate.ResumePath).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "application/pdf";
            }
            else if (Path.GetExtension(candidate.ResumePath).Equals(".docx", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }

            return File(fileBytes, contentType, fileName);
        }

        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            var totalCandidates = _context.Candidates.Count();
            var candidatesByDepartment = _context.Candidates
                .GroupBy(c => c.Department)
                .Select(g => new
                {
                    Department = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToList();

            return Ok(new
            {
                TotalCandidates = totalCandidates,
                CandidatesByDepartment = candidatesByDepartment
            });
        }


    }


}
