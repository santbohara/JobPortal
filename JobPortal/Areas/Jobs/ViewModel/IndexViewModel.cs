using JobPortal.Areas.Jobs.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Areas.Jobs.ViewModel
{
    public class IndexViewModel
    {
        [Display(Name = "Job Title")]
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Application Deadline")]
        public DateTime? ExpireDate { get; set; }

        [Display(Name = "Job Category")]
        public string? Category { get; set; }

        [Display(Name = "Job Qualification")]
        public string? JobQualification { get; set; }

        [Display(Name = "Job Type")]
        public string? JobType { get; set; }

        [Display(Name = "Salary Type")]
        public string? SalaryType { get; set; }

        [Display(Name = "Salary Range")]
        public string? SalaryRange { get; set; }

        [Display(Name = "Job Experience")]
        public string? JobExperience { get; set; }

        [Display(Name = "Job Shift")]
        public string? JobShift { get; set; }

        [Display(Name = "Job Level")]
        public string? JobLevel { get; set; }

        public string? CreatedBy { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Job Description")]
        public string? JobDescription { get; set; }

        [Display(Name = "Job Specification")]
        public string? JobSpecification { get; set; }

        [Display(Name = "Publish")]
        public bool IsPublished { get; set; }

        public List<Job>? JobList { get; set; }

        public List<SelectListItem>? JobCategories { get; set; }

        public List<SelectListItem>? JobQualifications { get; set; }

        public List<SelectListItem>? JobTypes { get; set; }

        public List<SelectListItem>? SalaryTypes { get; set; }

        public List<SelectListItem>? SalaryRanges { get; set; }

        public List<SelectListItem>? JobExperiences { get; set; }

        public List<SelectListItem>? JobShifts { get; set; }

        public List<SelectListItem>? JobLevels { get; set; }

    }
}
