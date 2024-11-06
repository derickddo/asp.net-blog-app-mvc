using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services.UserService;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostService _postService;

    public HomeController(ILogger<HomeController> logger, IPostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    public async Task<IActionResult> Index()
    {
        // get all posts
        List<Blog> blogs = await _postService.GetPostsAsync();
        return View(blogs);
    }

    public IActionResult Privacy()
    {
        return View();
    }





    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
