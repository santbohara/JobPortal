using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data;
using JobPortal.Models;
using JobPortal.Areas.Jobs.ViewModel;

namespace JobPortal.Controllers
{
    public class ApplyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Apply/{slug}")]

        public IActionResult Index(string slug)
        {
            if (slug == null || _context.JobApplication == null)
            {
                return Problem("Id not found");
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
                          where j.Slug == slug
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
                return Problem("Job not found");
            }

            return View(result);
        }
    }
}
