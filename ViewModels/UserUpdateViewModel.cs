using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels;

public class UserUpdateViewModel
{   
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public IFormFile? Avatar { get; set; }

}
