using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobPortal.Areas.Config.Models;
using JobPortal.Data;
using JobPortal.Areas.Config.ViewModels;

namespace JobPortal.Areas.Config.Controllers
{
    [Area("Config")]
    public class JobQualificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobQualificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Config/JobQualifications
        public async Task<IActionResult> Index()
        {
            JobQualificationViewModel list = new()
            {
                jobQualification = await _context.JobQualification.ToListAsync()
            };

            return View(list);
        }

        public async Task<IActionResult> Add([Bind("Title", "IsActive")] JobQualificationViewModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    JobQualification category = new()
                    {
                        Title = input.Title,
                        IsActive = input.IsActive,
                        CreatedBy = User.Identity?.Name ?? "N/A",
                        CreatedAt = DateTime.Now
                    };

                    _context.Add(category);

                    TempData["Success"] = "Record Added Successfully!";
                    await _context.SaveChangesAsync();
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
            var category = _context.JobQualification.Find(id);

            return Json(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id", "Title", "IsActive")] JobQualificationViewModel input)
        {
            if (id != input.Id)
            {
                return NotFound();
            }

            var dataToEdit = await _context.JobQualification.FindAsync(id);

            if (dataToEdit != null)
            {
                if (ModelState.IsValid)
                {
                    dataToEdit.Title = input.Title;
                    dataToEdit.IsActive = input.IsActive;

                    _context.Update(dataToEdit);

                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                Problem("Record Not found to edit");
            }
            
            TempData["Success"] = "Record Updated Successfully!";

            return RedirectToAction(nameof(Index));

        }
    }
}
