using JobPortal.Areas.Config.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Areas.Jobs.ViewModel
{
    public class JobViewModel
    {
        [Key]
        public int Id { get; set; }

        public string? Slug { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Application Deadline")]
        public DateTime? ExpireDate { get; set; }

        [Required]
        [Display(Name = "Job Category")]
        public string Category { get; set; }

        [Required]
        [Display(Name = "Job Qualification")]
        public string JobQualification { get; set; }

        [Required]
        [Display(Name = "Job Type")]
        public string JobType { get; set; }

        [Required]
        [Display(Name = "Salary Type")]
        public string SalaryType { get; set; }

        [Required]
        [Display(Name = "Salary Range")]
        public string SalaryRange { get; set; }

        [Required]
        [Display(Name = "Job Experience")]
        public string JobExperience { get; set; }

        [Required]
        [Display(Name = "Job Shift")]
        public string JobShift { get; set; }

        [Required]
        [Display(Name = "Job Level")]
        public string JobLevel { get; set; }

        public string? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        [Required]
        [Display(Name = "Job Specification")]
        public string JobSpecification { get; set; }

        [Display(Name = "Publish")]
        public bool IsPublished { get; set; }

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
