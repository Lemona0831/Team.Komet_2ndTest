using Issue_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Issue_Tracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Issue> Issues { get; set; }
    }
}
