using JobPortal.Data;
using JobPortal.Areas.Jobs.Models;
using JobPortal.Areas.Jobs.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using JobPortal.Models;

namespace JobPortal.Areas.Jobs.Controllers
{
    [Area("Jobs")]
    [Authorize]
    public class AddController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AdminUser> _user;

        public AddController(ApplicationDbContext db, UserManager<AdminUser> user)
        {
            _db = db;
            _user = user;
        }
        public IActionResult Index()
        {
            AddViewModel lists = new()
            {
                JobCategories = _db.JobCategory.Where(c => c.IsActive == true).ToList(),
                JobQualifications = _db.JobQualification.Where(c => c.IsActive == true).ToList(),
                JobTypes = _db.JobType.Where(c => c.IsActive == true).ToList(),
                SalaryTypes = _db.SalaryType.Where(c => c.IsActive == true).ToList(),
                SalaryRanges = _db.SalaryRange.Where(c => c.IsActive == true).ToList(),
                JobExperiences = _db.JobExperience.Where(c => c.IsActive == true).ToList(),
                JobShifts = _db.JobShift.Where(c => c.IsActive == true).ToList(),
                JobLevels = _db.JobLevel.Where(c => c.IsActive == true).ToList(),
            };

            return View(lists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddViewModel input)
        {
            var user = await _user.GetUserAsync(User);

            JobsList job = new()
            {
                Title = input.Title,
                Category = input.Category,
                JobQualifications = input.JobQualification,
                JobType = input.JobType,
                SalaryType = input.JobType,
                SalaryRange = input.JobType,
                JobExperience = input.JobType,
                JobShift = input.JobType,
                JobLevel = input.JobType,
                IsPublished = false,
                ExpireDate = (DateTime)input.ExpireDate,
                CreatedBy = user.UserName,
                CreatedAt = input.CreatedAt,

            };

            _db.Add(job);

            TempData["Success"] = "Job Added Successfully!";
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Dashboard", new { area = " " });

        }
    }
}
