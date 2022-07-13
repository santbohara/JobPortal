using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Areas.Users.ViewModels
{
    public class AddUserViewModel
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "{0} must be {1} character(s) in length.")]
        public string PhoneNumber { get; set; }

        [Display(Name="Active")]
        public bool IsActive { get; set; }
        
        public string Role { get; set; }

        [NotMapped]
        public List<SelectListItem>? Roles { get; set; }
    }
}
