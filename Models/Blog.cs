using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Blog
{

    public int Id { get; set; }

    [StringLength(50)]
    [Required]
    public string Title { get; set; } = "";

    [Required]
    public string Content { get; set; } = "";

    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required]
    public User? Authour { get; set; } 

    // image
    [Required]
    [DataType(DataType.ImageUrl)]
    public string Image { get; set; } = ""; 
}
