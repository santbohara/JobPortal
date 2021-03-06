using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Areas.Users.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public int Status { get; set; }

        public bool IsActive { get; set; }

        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "{0} must be {1} character(s) in length.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        public IFormFile ProfilePicture { get; set; }

        public string ProfilePictureUrl { get; set; }

        public bool EmailConfirmed { get; set; }

        [NotMapped]
        public List<SelectListItem>? Roles { get; set; }
    }
}
