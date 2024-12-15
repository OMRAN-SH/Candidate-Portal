using System.ComponentModel.DataAnnotations;

namespace CandidatePortal.Models
{
    public class Candidate
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Range(0, 50)] // Assuming max 50 years of experience
        public int YearsOfExperience { get; set; }

        [Required]
        public Department Department { get; set; } // Enum type for Department

        [Required]
        public string ResumePath { get; set; } = string.Empty; // File path for uploaded resume
    }

    public enum Department
    {
        IT,
        HR,
        Finance
    }
}
