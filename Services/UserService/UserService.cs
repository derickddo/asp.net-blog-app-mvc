using System;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services.FileService;
using WebApplication1.ViewModels;

namespace WebApplication1.Services.UserService;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IFileService _fileService;

    public UserService(
        ApplicationDbContext context,
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IFileService fileService
    )
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
        _fileService = fileService;
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

    public async Task<User> UpdateUserAsync(UserUpdateViewModel user, string id)
    {
        var userToUpdate = await _userManager.FindByIdAsync(id);
        if (userToUpdate == null)
        {
            throw new Exception("User not found");
        }

        userToUpdate.FirstName = user.FirstName!;
        userToUpdate.LastName = user.LastName!;
        userToUpdate.Email = user.Email;
        userToUpdate.UserName = user.UserName;
    
        // check if the avatar has changed
        var updateAvatar = await _fileService.UpdateFileAsync(userToUpdate.Avatar, user.Avatar!);

        if (!string.IsNullOrEmpty(updateAvatar))
        {
            userToUpdate.Avatar = updateAvatar;
        }

        await _userManager.UpdateAsync(userToUpdate);
        return userToUpdate;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if(user == null) throw new Exception("User not found");
        return user;
    }
}

