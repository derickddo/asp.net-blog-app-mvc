using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels;

public class PostViewModel
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";

    // image
    [Required]
    public IFormFile? Image { get; set; } 
}
