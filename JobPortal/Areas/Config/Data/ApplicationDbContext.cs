using JobPortal.Areas.Config.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Areas.Config.Data
{
    public class ApplicationDbContext
    {
        public ApplicationDbContext()
            : base()
        {
        }

        public DbSet<JobCategory> JobCategory { get; set; }
    }
}
