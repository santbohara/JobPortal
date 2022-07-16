using JobPortal.Areas.Config.Models;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Areas.Config.ViewModels
{
    public class JobTypeViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public List <JobType>? jobType { get; set; }
    }
}
