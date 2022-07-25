using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data;
using JobPortal.Models;

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

            var apply = (from j in _context.Job.Where(x => x.Slug.Equals(slug)) select j).FirstOrDefault();

            if (apply == null)
            {
                return Problem("Job not found");
            }

            return View(apply);
        }
    }
}
