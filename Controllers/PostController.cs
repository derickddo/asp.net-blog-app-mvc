using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services.FileService;
using WebApplication1.Services.UserService;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        public PostController(IPostService postService, IUserService userService, IFileService fileService)
        {
            _postService = postService;
            _userService = userService;
            _fileService = fileService;
        }

        // GET: PostController
        [Authorize] // only authenticated users can create the posts
        public IActionResult CreatePost()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // prevent CSRF attacks
        [Authorize] // only authenticated users can create a post
        public async Task<IActionResult> CreatePost(PostViewModel post)
        {
            if (!ModelState.IsValid) return View(post);
            
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null) return Unauthorized();  // In case the user ID is not found
            

            var currentUser = await _userService.GetUserByIdAsync(currentUserId);

            if (currentUser == null)
            {
                return Unauthorized();  // In case the user is not found in the database
            }

            // Save the image
            var postImage = await _fileService.SaveFileAsync(post.Image!);
            
            // Create a new blog
            var newBlog = new Blog
            {
                Title = post.Title,
                Content = post.Content,
                Authour = currentUser,
                CreatedAt = DateTime.Now,
                Image = postImage 
            };

            // Save the blog
            var blog = await _postService.SavePostAsync(newBlog);

            return RedirectToAction("PostDetail", "Post", new { id = blog.Id });

        }

        // GET: PostController/PostDetail/5}
        [HttpGet]
        public async Task<IActionResult> PostDetail(int id)
        {
           var blog = await _postService.GetPostByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            var authour = blog.Authour != null ? await _userService.GetUserByIdAsync(blog.Authour.Id) : new User();
            ViewBag.Authour = authour;
            return View(blog);
        }

        // GET: PostController/Edit/5
        [HttpGet]
        [Authorize] // only authenticated users can edit the post
        public async Task<IActionResult> EditPost(int id)
        {
            var blog = await _postService.GetPostByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog); 
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // only authenticated users can edit the post
        public async Task<IActionResult> EditPost(int id,  Blog blog)
        {
            if (!ModelState.IsValid) return View(blog);
            // get form data
            var data = HttpContext.Request.Form;
            Console.WriteLine(data);

            if (data == null)
            {
                return BadRequest();
            }
            string title = data["Title"].ToString();
            string content = data["Content"].ToString();
            var image = data.Files["Image"];

            var post = new PostViewModel
            {
                Title = title,
                Content = content,
                Image = image
            };

            var updatedBlogPost = await _postService.UpdatePostAsync(id, post);
            return RedirectToAction("PostDetail", "Post", new { id = updatedBlogPost.Id });
        }

        // GET: PostController/Delete/5
        [HttpGet]
        [Authorize] // only authenticated users can delete the post
        public async Task<IActionResult> ConfirmDeletePost(int id)
        {
            var blog = await _postService.GetPostByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // only authenticated users can delete the post
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePostAsync(id);
            return RedirectToAction("Index", "Home");
        }

    }
}
