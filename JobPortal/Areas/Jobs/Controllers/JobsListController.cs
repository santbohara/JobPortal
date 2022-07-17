using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobPortal.Areas.Jobs.Models;
using JobPortal.Data;

namespace JobPortal.Areas.Jobs.Controllers
{
    [Area("Jobs")]
    public class JobsListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobsListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jobs/JobsList
        public async Task<IActionResult> Index()
        {
              return _context.JobsList != null ? 
                          View(await _context.JobsList.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.JobsList'  is null.");
        }

        // GET: Jobs/JobsList/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.JobsList == null)
            {
                return NotFound();
            }

            var jobsList = await _context.JobsList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobsList == null)
            {
                return NotFound();
            }

            return View(jobsList);
        }

        // GET: Jobs/JobsList/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.JobsList == null)
            {
                return NotFound();
            }

            var jobsList = await _context.JobsList.FindAsync(id);
            if (jobsList == null)
            {
                return NotFound();
            }
            return View(jobsList);
        }

        // POST: Jobs/JobsList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Category,JobQualifications,JobType,SalaryType,SalaryRange,JobExperience,JobShift,JobLevel,IsPublished,ExpireDate,CreatedBy,CreatedAt")] JobsList jobsList)
        {
            if (id != jobsList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobsList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobsListExists(jobsList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jobsList);
        }

        private bool JobsListExists(Guid id)
        {
          return (_context.JobsList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
