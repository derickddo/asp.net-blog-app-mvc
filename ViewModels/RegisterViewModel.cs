using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Last Nmae is required")]
    public string LastName { get; set; } = "";
    [Required(ErrorMessage = "Username is required.")]
    [EmailAddress]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]

    public required string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

    public required string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [DataType(DataType.Text)]
    public required string UserName { get; set; }
}
