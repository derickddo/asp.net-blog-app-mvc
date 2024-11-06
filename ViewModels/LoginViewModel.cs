using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels;

public class LoginViewModel
{
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [DataType(DataType.Password)]
    public required string Password { get; set; }
}
