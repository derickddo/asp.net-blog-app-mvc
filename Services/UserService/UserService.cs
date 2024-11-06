using System;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services.UserService;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserService(
        ApplicationDbContext context,
        SignInManager<User> signInManager,
        UserManager<User> userManager
    )
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
    }


    public async Task<(bool, string)> LoginUserAsync(LoginViewModel model)
    {
        //find user using email
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return (false, "Invalid login attempt");
        }
        //sign in user
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        if (result.Succeeded)
        {
            return (true, "");
        }
        else
        {
            return (false, "Invalid login attempt");
        }
    }

    public async Task<(bool, List<string>)> RegisterUserAsync(RegisterViewModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }
        User user = new()
        {
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };


        var results = await _userManager.CreateAsync(user, model.Password);

        if (results.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return (true, []);
        }
        else
        {
            return (false, results.Errors.Select(e => e.Description).ToList());

        }

    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return user;
    }

    public Task LogoutUserAsync()
    {
        return _signInManager.SignOutAsync();
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return user;
        }
        else
        {
            throw new Exception("Failed to update user");
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if(user == null) throw new Exception("User not found");
        return user;
    }
}

