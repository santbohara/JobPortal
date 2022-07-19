using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal.Areas.Jobs.Models;
using JobPortal.Data;
using JobPortal.Models;
using Microsoft.AspNetCore.Identity;
using JobPortal.Areas.Jobs.ViewModel;
using JobPortal.Areas.Config.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortal.Areas.Jobs.Controllers
{
    [Area("Jobs")]
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AdminUser> _user;

        public JobController(ApplicationDbContext context, UserManager<AdminUser> user)
        {
            _context = context;
            _user = user;
        }

        // GET: Jobs/JobsList
        public async Task<IActionResult> Index()
        {
            return _context.Job != null ?
                        View(await _context.Job.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.JobsList'  is null.");
        }

        public IActionResult Add()
        {
            JobViewModel lists = new()
            {
                JobCategories = JobCategoryList(),
                JobQualifications = JobQualificationsList(),
                JobTypes = JobTypesList(),
                SalaryTypes = SalaryTypesList(),
                SalaryRanges = SalaryRangesList(),
                JobExperiences = JobExperiencesList(),
                JobShifts = JobShiftsList(),
                JobLevels = JobLevelsList(),
            };

            return View(lists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Title,Category,JobQualification,JobType,SalaryType,SalaryRange,JobExperience,JobShift,JobLevel,IsPublished,ExpireDate,JobDescription,JobSpecification")] JobViewModel input)
        {
            var user = await _user.GetUserAsync(User);

            if (input.ExpireDate == null)
            {
                JobViewModel lists = new()
                {
                    JobCategories = JobCategoryList(),
                    JobQualifications = JobQualificationsList(),
                    JobTypes = JobTypesList(),
                    SalaryTypes = SalaryTypesList(),
                    SalaryRanges = SalaryRangesList(),
                    JobExperiences = JobExperiencesList(),
                    JobShifts = JobShiftsList(),
                    JobLevels = JobLevelsList(),
                };

                TempData["Danger"] = "Application Deadline is required!";

                return View(lists);
            }

            Job job = new()
            {
                Title = input.Title,
                Category = input.Category,
                JobQualification = input.JobQualification,
                JobType = input.JobType,
                SalaryType = input.SalaryType,
                SalaryRange = input.SalaryRange,
                JobExperience = input.JobExperience,
                JobShift = input.JobShift,
                JobLevel = input.JobLevel,
                IsPublished = false,
                ExpireDate = (DateTime)input.ExpireDate,
                CreatedBy = user.UserName,
                CreatedAt = input.CreatedAt,

            };

            _context.Add(job);

            TempData["Success"] = "Job Added Successfully!";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        // GET: Jobs/JobsList/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var jobsList = await _context.Job
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
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var edit = await _context.Job.FindAsync(id);

            if (edit == null)
            {
                return NotFound();
            }

            var result = new JobViewModel
            {
                Title = edit.Title,
                Category = edit.Category,
                JobQualification = edit.JobQualification,
                JobType = edit.JobType,
                SalaryType = edit.SalaryType,
                SalaryRange = edit.SalaryRange,
                JobExperience = edit.JobExperience,
                JobShift = edit.JobShift,
                JobLevel = edit.JobLevel,
                IsPublished = false,
                ExpireDate = (DateTime)edit.ExpireDate,
                JobDescription = edit.JobDescription,
                JobSpecification = edit.JobSpecification,
                JobCategories = JobCategoryList(),
                JobQualifications = JobQualificationsList(),
                JobTypes = JobTypesList(),
                SalaryTypes = SalaryTypesList(),
                SalaryRanges = SalaryRangesList(),
                JobExperiences = JobExperiencesList(),
                JobShifts = JobShiftsList(),
                JobLevels = JobLevelsList()
            };

            return View(result);
        }

        // POST: Jobs/JobsList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Title,Category,JobQualification,JobType,SalaryType,SalaryRange,JobExperience,JobShift,JobLevel,IsPublished,ExpireDate,JobDescription,JobSpecification")] JobViewModel job)
        {
            var user = await _user.GetUserAsync(User);

            try
            {
                var abc = await _context.Job.FindAsync(job.Id);

                if (abc == null)
                {
                    return NotFound();
                }
                else
                {
                    abc.Title = job.Title;
                    abc.ExpireDate = (DateTime)job.ExpireDate;
                    abc.JobDescription = job.JobDescription;
                    abc.JobSpecification = job.JobSpecification;
                    abc.Category = job.Category;
                    abc.JobQualification = job.JobQualification;
                    abc.JobType = job.JobType;
                    abc.SalaryType = job.SalaryType;
                    abc.SalaryRange = job.SalaryRange;
                    abc.JobExperience = job.JobExperience;
                    abc.JobShift = job.JobShift;
                    abc.JobLevel = job.JobLevel;
                    abc.IsPublished = job.IsPublished;
                    abc.CreatedBy = user.UserName;

                    _context.Update(abc);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobsListExists(job.Id))
                {
                    return NotFound();
    }
                else
                {
                    throw;
                }
            }
        }

        private bool JobsListExists(Guid id)
{
    return (_context.Job?.Any(e => e.Id == id)).GetValueOrDefault();
}

private List<SelectListItem> JobCategoryList()
{
    var items = _context.JobCategory.Where(x => x.IsActive == true).ToList();

    var itemsList = items.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToList();

    return itemsList;
}
private List<SelectListItem> JobQualificationsList()
{
    var items = _context.JobQualification.Where(x => x.IsActive == true).ToList();

    var itemsList = items.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToList();

    return itemsList;
}
private List<SelectListItem> JobTypesList()
{
    var items = _context.JobType.Where(x => x.IsActive == true).ToList();

    var itemsList = items.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToList();

    return itemsList;
}
private List<SelectListItem> SalaryTypesList()
{
    var items = _context.SalaryType.Where(x => x.IsActive == true).ToList();

    var itemsList = items.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToList();

    return itemsList;
}
private List<SelectListItem> SalaryRangesList()
{
    var items = _context.SalaryRange.Where(x => x.IsActive == true).ToList();

    var itemsList = items.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToList();

    return itemsList;
}
private List<SelectListItem> JobExperiencesList()
{
    var items = _context.JobExperience.Where(x => x.IsActive == true).ToList();

    var itemsList = items.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToList();
    return itemsList;
}
private List<SelectListItem> JobShiftsList()
{
    var items = _context.JobShift.Where(x => x.IsActive == true).ToList();

    var itemsList = items.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToList();

    return itemsList;
}
private List<SelectListItem> JobLevelsList()
{
    var items = _context.JobLevel.Where(x => x.IsActive == true).ToList();

    var itemsList = items.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToList();

    return itemsList;
}
    }
}
