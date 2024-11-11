using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services.UserService;
using WebApplication1.ViewModels;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using BlogApp.ViewModels;


namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /Auth/Register
        public IActionResult Register()
        {

            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        public async Task<IActionResult> RegisterHandler(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Call the RegisterUserAsync method from the service
                    var (Succeeded, Errors) = await _userService.RegisterUserAsync(model);

                    if (Succeeded)
                    {   var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.UserName),
                            new Claim(ClaimTypes.Email, model.Email),
                            new Claim(ClaimTypes.GivenName, model.FirstName),
                            new Claim(ClaimTypes.Surname, model.LastName)
                        };
                        
                        var claimsIdentity = new ClaimsIdentity(
                            claims, 
                            CookieAuthenticationDefaults.AuthenticationScheme
                        );

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), 
                            authProperties
                        );
                        
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in Errors)
                        {
                            ModelState.AddModelError(string.Empty, error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }

            return View(model);
        }

        // GET: Auth/Login
        public IActionResult Login(string? returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl; // store the return url in the view data

            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {

            // if the return url is null, set it to the home page
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Url.Content("~/");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var (succeeded, error) = await _userService.LoginUserAsync(model);

                    if (succeeded)
                    {   
                        // Redirect to the return url
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        // GET: /Auth/Logout
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUserAsync();
            return RedirectToAction("Login", "Auth");
        }

        public async Task<IActionResult> UpdateUser(string id)
        {   
            var user = await _userService.GetUserByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(string id, User user)

        {   // get the avatar from the form
            var avatar = Request.Form.Files["Avatar"] ?? null;

            var updateUser = new UserUpdateViewModel
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Avatar = avatar
            };

            var updatedUser = await _userService.UpdateUserAsync(updateUser, id);
            return RedirectToAction("Auth", "UpdateUser", new { id = updatedUser.Id });
        }
    }
}

