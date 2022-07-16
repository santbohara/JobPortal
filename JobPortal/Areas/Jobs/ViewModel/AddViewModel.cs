using JobPortal.Areas.Config.Models;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Areas.Jobs.ViewModel
{
    public class AddViewModel
    {
        [Required]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpireDate { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string JobQualification { get; set; }

        [Required]
        public string JobType { get; set; }

        [Required]
        public string SalaryType { get; set; }

        [Required]
        public string SalaryRange { get; set; }

        [Required]
        public string JobExperience { get; set; }

        [Required]
        public string JobShift { get; set; }

        [Required]
        public string JobLevel { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        [Required]
        [Display(Name = "Job Specification")]
        public string JobSpecification { get; set; }

        public List<JobCategory> JobCategories { get; set; }

        public List<JobQualification> JobQualifications { get; set; }

        public List<JobType> JobTypes { get; set; }

        public List<SalaryType> SalaryTypes { get; set; }

        public List<SalaryRange> SalaryRanges { get; set; }

        public List<JobExperience> JobExperiences { get; set; }

        public List<JobShift> JobShifts { get; set; }

        public List<JobLevel> JobLevels { get; set; }
    }
}
