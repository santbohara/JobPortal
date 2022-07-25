using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal.Areas.Jobs.Models;
using JobPortal.Data;
using JobPortal.Models;
using Microsoft.AspNetCore.Identity;
using JobPortal.Areas.Jobs.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Slugify;

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
        public IActionResult Index(
            string Category,
            string JobQualification,
            string JobExperience,
            string Joblevel,
            bool? IsActive,
            bool? NotPublished,
            DateTime? PostedDateFrom,
            DateTime? PostedDateTo,
            bool? Expired
        )
        {
            var filter = from j in _context.Job select j;

            if (Category != null) filter = filter.Where(s => s.Category.Equals(Category));
            if (JobQualification != null) filter = filter.Where(s => s.JobQualification.Equals(JobQualification));
            if (JobExperience != null) filter = filter.Where(s => s.JobExperience.Equals(JobExperience));
            if (Joblevel != null) filter = filter.Where(s => s.JobLevel.Equals(Joblevel));
            if (IsActive != null) filter = filter.Where(s => s.IsPublished == IsActive && s.ExpireDate.Date > DateTime.Today);
            if (NotPublished != null) filter = filter.Where(s => s.IsPublished != NotPublished);
            if (PostedDateFrom != null && PostedDateTo != null) filter = filter.Where(s => s.CreatedAt.Date >= PostedDateFrom && s.CreatedAt.Date <= PostedDateTo);
            if (Expired != null) filter = filter.Where(s => s.ExpireDate.Date < DateTime.Today && s.IsPublished == true);

            //var d = DateTime.Today;

            IndexViewModel lists = new()
            {
                JobCategories = JobCategoryList(),
                JobQualifications = JobQualificationsList(),
                JobTypes = JobTypesList(),
                SalaryTypes = SalaryTypesList(),
                SalaryRanges = SalaryRangesList(),
                JobExperiences = JobExperiencesList(),
                JobShifts = JobShiftsList(),
                JobLevels = JobLevelsList(),
                JobList = filter.ToList()
            };

            ViewData["BaseUrl"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            return View(lists);
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
            if (!ModelState.IsValid)
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

            var user = await _user.GetUserAsync(User);

            //Get Slug
            var slug = GetSlug(input.Title);

            if (slug == null)
            {
                TempData["Danger"] = "Slug already used, try with new Job Title";

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

            //Else process to add
            Job job = new()
            {
                Title = input.Title,
                Slug = slug,
                Category = input.Category,
                JobQualification = input.JobQualification,
                JobType = input.JobType,
                SalaryType = input.SalaryType,
                SalaryRange = input.SalaryRange,
                JobExperience = input.JobExperience,
                JobShift = input.JobShift,
                JobLevel = input.JobLevel,
                JobDescription = input.JobDescription,
                JobSpecification = input.JobSpecification,
                IsPublished = false,
                ExpireDate = (DateTime)input.ExpireDate,
                CreatedBy = user.UserName,
                CreatedAt = (DateTime)input.CreatedAt,

            };

            _context.Add(job);

            TempData["Success"] = "Job Added Successfully!";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Jobs/JobsList/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return Problem(id.ToString());
            }

            var result = (from j in _context.Job
                          join jc in _context.JobCategory on j.Category equals jc.Id.ToString()
                          join jq in _context.JobQualification on j.JobQualification equals jq.Id.ToString()
                          join jt in _context.JobType on j.JobType equals jt.Id.ToString()
                          join st in _context.SalaryType on j.SalaryType equals st.Id.ToString()
                          join sr in _context.SalaryRange on j.SalaryRange equals sr.Id.ToString()
                          join je in _context.JobExperience on j.JobExperience equals je.Id.ToString()
                          join js in _context.JobShift on j.JobShift equals js.Id.ToString()
                          join jl in _context.JobLevel on j.JobLevel equals jl.Id.ToString()
                          where j.Id == id
                          select new JobViewModel
                          {
                              Title = j.Title,
                              Slug = j.Slug,
                              IsPublished = j.IsPublished,
                              ExpireDate = j.ExpireDate,
                              CreatedBy = j.CreatedBy,
                              CreatedAt = j.CreatedAt,
                              JobDescription = j.JobDescription,
                              JobSpecification = j.JobSpecification,
                              Category = jc.Title,
                              JobQualification = jq.Title,
                              JobType = jt.Title,
                              SalaryType = st.Title,
                              SalaryRange = sr.Title,
                              JobExperience = je.Title,
                              JobShift = js.Title,
                              JobLevel = jl.Title
                          }).FirstOrDefault();

            if (result == null)
            {
                return Problem(id.ToString());
            }

            ViewData["BaseUrl"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            return View(result);
        }

        // GET: Jobs/JobsList/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
                IsPublished = edit.IsPublished,
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

                    if (!ModelState.IsValid)
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
                    }

                    TempData["Success"] = "Job updated successfully!";
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

        private bool JobsListExists(int id)
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
        public string GetSlug(string Title)
        {
            string? finalSlug = null;

            //Slug helper
            SlugHelper helper = new();

            //Generate Slug for Job Post
            var slug = helper.GenerateSlug(Title);

            //Get last ID to concatenate in slug for uniqueness of slug
            var lastId = _context.Job.OrderByDescending(a => a.Id).FirstOrDefault();

            //concatenate id and slug
            if (lastId == null)
            {
                finalSlug = slug;
            }
            else
            {
                //Get Last id and increament by 1
                int id = lastId.Id;

                var NewId = id + 1;

                finalSlug = slug + "-" + NewId;
            }

            //Also re-check if final slug is already being used
            var slugCheck = from s in _context.Job.Where(s => s.Slug == finalSlug) select s;

            if (slugCheck.Any())
            {
                return null;
            }
            else
            {
                return finalSlug;
            }
        }
    }
}
