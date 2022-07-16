﻿using JobPortal.Areas.Config.Models;
using JobPortal.Areas.Config.ViewModels;
using JobPortal.Data;
using JobPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Areas.Config.Controllers
{
    [Area("Config")]
    public class SalaryRangeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AdminUser> _user;

        public SalaryRangeController(ApplicationDbContext db, UserManager<AdminUser> user)
        {
            _db = db;
            _user = user;
        }

        public async Task<IActionResult> Index()
        {
            SalaryRangeViewModel list = new()
            {
                SalaryRange = await _db.SalaryRange.ToListAsync()
            };

            return View(list);
        }

        [HttpPost]

        public async Task<IActionResult> Add([Bind("Title", "IsActive")] SalaryRangeViewModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SalaryRange result = new()
                    {
                        Title = input.Title,
                        IsActive = input.IsActive,
                        CreatedBy = User.Identity?.Name ?? "N/A",
                        CreatedAt = DateTime.Now
                    };

                    _db.Add(result);

                    TempData["Success"] = "Record Added Successfully!";
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
            var result = _db.SalaryRange.Find(id);

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id", "Title", "IsActive")] SalaryRangeViewModel input)
        {
            if (id != input.Id)
            {
                return NotFound();
            }

            var dataToEdit = await _db.SalaryRange.FindAsync(id);

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
                Problem("Record Not found to edit");
            }

            return RedirectToAction(nameof(Index));

        }
    }
}
