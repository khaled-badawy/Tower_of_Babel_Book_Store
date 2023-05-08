using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using My_Final_Project.Models;
using My_Final_Project.Repositry;
using My_Final_Project.ViewModels;
using static My_Final_Project.Models.ApplicationUser;

namespace My_Final_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager; // Database User Manager
        private readonly SignInManager<ApplicationUser> signInManager; // Cookie sign in Manager
        private readonly IApplicationUserRepositry user;
        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, IApplicationUserRepositry user)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            this.user = user;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModel = new ApplicationUser();
                userModel.UserName= newUser.UserName;
                userModel.Email = newUser.Email;
                userModel.PasswordHash = newUser.Password;
                userModel.PhoneNumber = newUser.Phone;
                userModel.Address= newUser.Address;
                userModel.Age = newUser.Age;

                IdentityResult result =  await userManager.CreateAsync(userModel,newUser.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userModel, "customer"); // add role
                    await signInManager.SignInAsync(userModel, false); // create cookie , not persistent 
                    return RedirectToAction("Index", "Home");
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
                    bool iscustomer = await userManager.IsInRoleAsync(userModel, "customer");
                    if (found && iscustomer)
                    {
                        await signInManager.SignInAsync(userModel, userVM.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Login Failed");
                }
                else
                {
                    ModelState.AddModelError("", "No Account Found");
                }
            }
            return View(userVM);
        }
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            
            HttpContext.Session.Clear();
            
            return RedirectToAction("Index","Home");
        }
        [Authorize]
        public async Task<IActionResult> ProfileInfo()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ApplicationUser user = await userManager.FindByIdAsync(userID);
                RegisterViewModel currentUser = new RegisterViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Address = user.Address,
                    Age = user.Age,
                    Phone = user.PhoneNumber,
                    Gender = (int)user.gender,

                };
                return View(currentUser);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult EditProfile()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel updateuser)
        {
            if (ModelState.IsValid)
            {
                var userID = User.FindFirstValue("UserID");
                ApplicationUser user = await userManager.FindByIdAsync(userID);
                if (user != null)
                {
                    await userManager.SetUserNameAsync(user, updateuser.UserName);
                    await userManager.SetEmailAsync(user, updateuser.Email);
                    await userManager.SetPhoneNumberAsync(user, updateuser.Phone);
                    user.Age = updateuser.Age;
                    user.gender = (Gender)updateuser.Gender;
                    user.Address = updateuser.Address;
                    await userManager.UpdateAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                return View(updateuser);
            }
            return View(updateuser);
        }
        [Authorize(Roles = "admin")]
        public IActionResult IndexAdmin()
        {
            ViewBag.userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var users = user.GetAll();
            return View(users);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Search(string search)
        {
            var users = user.GetAll();

            if (!String.IsNullOrEmpty(search))
            {
                users = users.Where(a => a.UserName.ToLower().Contains(search.ToLower())).ToList();
                ViewBag.SearchUser = search;
            }

            return View("_UsersTablePartial", users);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Delete(string id)
        {
            user.Delete(id);

            var users = user.GetAll();
            return RedirectToAction("IndexAdmin", users);
        }
    }
}
