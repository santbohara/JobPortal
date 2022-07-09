using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels
{
    public class JobsViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public DateTime ExpireDate { get; set; }
    }
}
