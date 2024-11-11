using System;
using BlogApp.ViewModels;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services.UserService;

public interface IUserService
{
    // registration method
    Task<(bool, List<string>)> RegisterUserAsync(RegisterViewModel model);

    // login method
    Task<(bool, string)> LoginUserAsync(LoginViewModel model);

    // get user by id
    Task<User?> GetUserByIdAsync(string id);

    // logout method
    Task LogoutUserAsync();

    Task<User> UpdateUserAsync(UserUpdateViewModel user, string id);
    Task<User> GetUserByEmailAsync(string email);
}
