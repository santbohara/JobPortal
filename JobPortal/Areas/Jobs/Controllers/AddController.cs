using JobPortal.Data;
using JobPortal.Areas.Jobs.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Areas.Jobs.Controllers
{
    [Area("Jobs")]
    [Authorize]
    public class AddController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AddController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            AddViewModel lists = new()
            {
                JobCategory     = _db.JobCategory.Where(c => c.IsActive == true).ToList(),
                JobQualification = _db.JobQualification.Where(c => c.IsActive == true).ToList(),
                JobType         = _db.JobType.Where(c => c.IsActive == true).ToList(),
                SalaryType      = _db.SalaryType.Where(c => c.IsActive == true).ToList(),
                SalaryRange     = _db.SalaryRange.Where(c => c.IsActive == true).ToList(),
                JobExperience   = _db.JobExperience.Where(c => c.IsActive == true).ToList(),
                JobShift        = _db.JobShift.Where(c => c.IsActive == true).ToList(),
                JobLevel        = _db.JobLevel.Where(c => c.IsActive == true).ToList(),
            };

            return View(lists);
        }
    }
}
