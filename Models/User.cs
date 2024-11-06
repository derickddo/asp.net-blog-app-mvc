using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models;

public class User : IdentityUser
{
    [Required]
    public string FirstName { get; set; } = "";
    [Required]
    public string LastName { get; set; } = "";

    // avatar
    [Required]
    [DataType(DataType.ImageUrl)]
    public string Avatar { get; set; } = "/img/user.png";

   
}
