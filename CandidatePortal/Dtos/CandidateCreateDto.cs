using CandidatePortal.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CandidatePortal.Dtos
{
    public class CandidateCreateDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Range(0, 50)]
        public int YearsOfExperience { get; set; }

        [Required]
        public Department Department { get; set; }

        [Required]
        public IFormFile Resume { get; set; } // For file upload
    }
}
