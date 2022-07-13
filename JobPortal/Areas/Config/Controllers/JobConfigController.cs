using JobPortal.Areas.Config.Models;
using JobPortal.Areas.Config.ViewModels;
using JobPortal.Data;
using JobPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Areas.Config.Controllers
{
    [Area("Config")]
    public class JobConfigController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AdminUser> _user;

        public JobConfigController(ApplicationDbContext db, UserManager<AdminUser> user)
        {
            _db = db;
            _user = user;
        }

        public async Task<IActionResult> Index()
        {
            JobCategoryViewModel list = new()
            {
                jobCategories = await _db.JobCategory.ToListAsync()
            };

            return View(list);
        }

        [HttpPost]

        public async Task<IActionResult> Add([Bind("Title", "IsActive")] JobCategoryViewModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    JobCategory category = new()
                    {
                        Title = input.Title,
                        IsActive = input.IsActive,
                        CreatedBy = User.Identity?.Name ?? "N/A",
                        CreatedAt = DateTime.Now
                    };

                    _db.Add(category);

                    TempData["Success"] = "Job Category Added Successfully!";
                    await _db.SaveChangesAsync();
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return RedirectToAction(nameof(Index));
        }

        public JsonResult Edit(Guid id)
        {
            var category = _db.JobCategory.Find(id);

            return Json(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id", "Title", "IsActive")] JobCategoryViewModel input)
        {
            if (id != input.Id)
            {
                return NotFound();
            }

            var dataToEdit = await _db.JobCategory.FindAsync(id);

            if (dataToEdit != null)
            {
                if (ModelState.IsValid)
                {
                    dataToEdit.Title = input.Title;
                    dataToEdit.IsActive = input.IsActive;

                    _db.Update(dataToEdit);

                    await _db.SaveChangesAsync();
                }
            }
            else
            {
                Problem("Category Not found to edit");
            }

            return RedirectToAction(nameof(Index));

        }
    }
}
