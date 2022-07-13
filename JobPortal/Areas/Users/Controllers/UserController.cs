using JobPortal.Areas.Users.ViewModels;
using JobPortal.Data;
using JobPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortal.Areas.Users.Controllers
{
    [Area("Users")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AdminUser> _user;

        private readonly ApplicationDbContext _db;

        public UserController(UserManager<AdminUser> user, ApplicationDbContext db)
        {
            _user = user;
            _db = db;
        }

        public IActionResult Index()
        {
            var users = _user.Users.Select(c => new UserViewModel()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                UserName = c.UserName,
                PhoneNumber = c.PhoneNumber,
                IsActive = c.IsActive,
                Role = string.Join(",", _user.GetRolesAsync(c).Result.ToArray()),
            }).ToList();

            return View(users);
        }

        public IActionResult Add()
        {
            AddUserViewModel role = new()
            {
                Roles = GetRoles()
            };

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Email", "Password", "FirstName", "LastName", "PhoneNumber", "Role", "IsActive")] AddUserViewModel input)
        {
            if (input == null)
            {
                return View();
            }
            else
            {
                var check = await _user.FindByEmailAsync(input.Email);

                if (check == null)
                {
                    var usr = new AdminUser()
                    {
                        FirstName = input.FirstName,
                        LastName = input.LastName,
                        UserName = input.Email,
                        NormalizedUserName = input.Email,
                        Email = input.Email,
                        PhoneNumber = input.PhoneNumber,
                        IsActive = input.IsActive,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,

                    };

                    var result = await _user.CreateAsync(usr, input.Password);
                    await _user.AddToRoleAsync(usr, input.Role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (result.Errors.Any())
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(input);
                    }
                }
                else
                {
                    AddUserViewModel role = new()
                    {
                        Roles = GetRoles()
                    };

                    TempData["Danger"] = "Email already registerd!";
                    return View(role);
                }
            }
        }

        public async Task<IActionResult> Edit(String Id)
        {
            var user = await _user.FindByIdAsync(Id);

            var editUser = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
            };

            if (user.ProfilePicture == null)
            {
                editUser.ProfilePictureUrl = "/static/default-img.jpg";
            }
            else
            {
                editUser.ProfilePictureUrl = $"data:image/png;base64,{Convert.ToBase64String(user.ProfilePicture)}";
            }

            var currentRoles = await _user.GetRolesAsync(user);

            var roles = _db.Roles.ToList();

            var rolesList = roles.Select(x =>
                            new SelectListItem
                            {
                                Text = x.Name,
                                Value = x.Name,
                                Selected = x.Id == Id
                            }).ToList();

            editUser.Roles = rolesList;

            return View(editUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id", "FirstName", "LastName", "PhoneNumber", "Role", "IsActive")] UserViewModel input)
        {
            //Check if input is null
            if (input == null)
            {
                return View();
            }
            else
            {
                var check = await _user.FindByIdAsync(input.Id);

                if (check == null)
                {
                    return NotFound();
                }
                else
                {
                    //find the role(s) of current user and remove it
                    var currentRoles = await _user.GetRolesAsync(check);
                    await _user.RemoveFromRolesAsync(check, currentRoles);

                    //Update User and Sync Role as well
                    check.FirstName = input.FirstName;
                    check.LastName = input.LastName;
                    check.PhoneNumber = input.PhoneNumber;
                    check.IsActive = input.IsActive;

                    var result = await _user.UpdateAsync(check);
                    await _user.AddToRoleAsync(check, input.Role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(input);
                    }
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(string Id, [Bind("ProfilePicture")] UserViewModel file)
        {
            var user = await _user.FindByIdAsync(Id);

            var stream = new MemoryStream();

            file.ProfilePicture?.CopyTo(stream);

            user.ProfilePicture = stream.ToArray();

            _db.Users.Update(user);

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetRoles()
        {
            var roles = _db.Roles.ToList();

            var rolesList = roles.Select(x =>
                            new SelectListItem
                            {
                                Text = x.Name,
                                Value = x.Name,
                            }).ToList();

            rolesList.Add(new SelectListItem { Text = "Choose...", Value = "", Selected = true });

            return rolesList;
        }
    }
}
