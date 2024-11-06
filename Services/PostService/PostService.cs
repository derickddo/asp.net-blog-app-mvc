using System;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services.FileService;
using WebApplication1.Services.UserService;
using WebApplication1.ViewModels;

namespace WebApplication1.Services.PostService;

public class PostService : IPostService
{
    // variable to hold the context
    private readonly ApplicationDbContext _context;
    private readonly IFileService _fileService;

    // constructor
    public PostService(ApplicationDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }
    

    // Delete post
    public async Task DeletePostAsync(int Id)
    {
        var blog = await _context.Blogs.FindAsync(Id);
        if (blog == null)
        {
            throw new Exception("Post not found");
        }
        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
        return;
    }

    // get post by Id   
    public async Task<Blog> GetPostByIdAsync(int Id)
    {
        var blog = await _context.Blogs.FindAsync(Id);
        if (blog == null)
        {
            throw new Exception("Post not found");
        }
        return blog;
    }

    // get all posts
    public async Task<List<Blog>> GetPostsAsync()
    {
        try
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            return blogs;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // save a post
    public async Task<Blog> SavePostAsync(Blog blog)
    {
        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
        return blog;
    }

    // Update post
    public async Task<Blog> UpdatePostAsync(int Id, PostViewModel postViewModel)
    {
        // find the post with the given Id
        var blog = await _context.Blogs.FindAsync(Id);

        // if post is not found
        if (blog == null) throw new Exception("Post not found");
        Console.WriteLine(blog.Image);
        // check if the image has changed
        var getImagePath = await _fileService.UpdateFileAsync(blog.Image, postViewModel.Image!);
        
        // update the image
        blog.Image = getImagePath;
    
        // update the post
        blog.Title = postViewModel.Title;
        blog.Content = postViewModel.Content;


        // save the changes
        _context.Blogs.Update(blog);
        await _context.SaveChangesAsync();

        return blog;
    }


}
