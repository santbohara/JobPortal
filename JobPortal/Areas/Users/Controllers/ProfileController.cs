using JobPortal.Areas.Users.ViewModels;
using JobPortal.Data;
using JobPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Areas.Users.Controllers
{

    [Area("Users")]
    public class ProfileController : Controller
    {
        private readonly UserManager<AdminUser> _user;
        private readonly ApplicationDbContext _db;

        public ProfileController(UserManager<AdminUser> user, ApplicationDbContext db)
        {
            _user = user;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _user.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Problem("User not found");
            }

            var editUser = new UserViewModel
            {
                Id = user.Id,
                ProfilePictureUrl = Convert.ToBase64String(user.ProfilePicture ?? Array.Empty<byte>()),
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = string.Join(",", _user.GetRolesAsync(user).Result.ToArray()),
            };

            return View(editUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(string Id, [Bind("ProfilePicture")] UserViewModel file)
        {
            if (file.ProfilePicture == null)
            {
                TempData["Danger"] = "Please select image to upload!";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var user = await _user.FindByIdAsync(Id);

                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    var stream = new MemoryStream();

                    file.ProfilePicture.CopyTo(stream);

                    user.ProfilePicture = stream.ToArray();

                    _db.Users.Update(user);

                    var result = await _db.SaveChangesAsync();

                    if (result > 0)
                    {
                        TempData["Success"] = "Picture updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Danger"] = "Update failed!";
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
        }

        public async Task<IActionResult> ChangePassword(String Id)
        {
            var user = await _user.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var editUser = new UserViewModel
            {
                Id = user.Id,
                ProfilePictureUrl = Convert.ToBase64String(user.ProfilePicture ?? Array.Empty<byte>()),
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Role = string.Join(",", _user.GetRolesAsync(user).Result.ToArray()),
            };

            return View(editUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string Id, [Bind("Email", "FirstName", "LastName", "Password", "ConfirmPassword")] UserViewModel input)
        {
            //Get User Details
            var user = await _user.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var token = await _user.GeneratePasswordResetTokenAsync(user);

            var result = await _user.ResetPasswordAsync(user, token, input.Password);

            if (result.Succeeded)
            {
                TempData["Success"] = "Password changed successfully!";

                return RedirectToAction("ChangePassword", new { Id });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(input);
        }
    }
}
