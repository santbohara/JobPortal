using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Jobs
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public int JobQualifications { get; set; }

        [Required]
        public int JobType { get; set; }

        [Required]
        public int SalaryType { get; set; }

        [Required]
        public int SalaryRange { get; set; }

        [Required]
        public int JobExperience { get; set; }

        [Required]
        public int JobShift { get; set; }

        [Required]
        public int JobLevel { get; set; }

        [Required]
        public bool IsPublished { get; set; }

        [Required]
        public DateTime ExpireDate { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

    }
}
