using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.Models;
using My_Final_Project.ViewModels;

namespace My_Final_Project.Controllers
{
    public class StaffController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager; // Database User Manager
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager; // Cookie sign in Manager

        public StaffController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModel = await userManager.FindByNameAsync(userVM.UserName);
                if (userModel != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userModel, userVM.Password);
                    bool isAdmin =  await userManager.IsInRoleAsync(userModel, "admin");
                    if (found && isAdmin)
                    {
                        await signInManager.SignInAsync(userModel, false);
                        return RedirectToAction("Index", "Dashboard");
                    }
                    ModelState.AddModelError("", " Cannot Login as It's For Staff Only");
                }
                else
                {
                    ModelState.AddModelError("", " Cannot Login as It's For Staff Only ");
                }
            }
            //return RedirectToAction("Login", "Staff");
            return View(userVM);
        }
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddRole()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddRole(RoleViewModel roleVM)
        {
            var role = new IdentityRole();
            role.Name = roleVM.Name;
            role.NormalizedName = roleVM.Name.ToUpper();
            var result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("IndexAdmin", "Account");
            }
            else
            {
                return View(roleVM);
            }
        }
        [Authorize(Roles = "admin")]
        public IActionResult AddUser()
        {
            var roles = new List<IdentityRole>();
            foreach (var role in roleManager.Roles)
            {
                roles.Add(role);
            }
            ViewBag.Roles = roles;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUser(AddingUserVM newUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModel = new ApplicationUser();
                userModel.UserName = newUser.UserName;
                userModel.Email = newUser.Email;
                userModel.PasswordHash = newUser.Password;
                userModel.PhoneNumber = newUser.Phone;
                userModel.Address = newUser.Address;
                userModel.Age = newUser.Age;
                //userModel.gender = newUser.Gender;

                IdentityResult result = await userManager.CreateAsync(userModel, newUser.Password);

                if (result.Succeeded)
                {
                    var roleName = roleManager.Roles.FirstOrDefault(r => r.Id == newUser.roleId);
                    await userManager.AddToRoleAsync(userModel, roleName.ToString()); // add role
                    return RedirectToAction("IndexAdmin", "Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                return View();
            }
            return View(newUser);
        }
    }
}
