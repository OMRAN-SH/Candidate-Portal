using Microsoft.EntityFrameworkCore;
using CandidatePortal.Models;

namespace CandidatePortal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Candidate> Candidates { get; set; }
    }
}
