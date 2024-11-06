using System;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services.UserService;

public interface IPostService
{
    // get all posts
    Task<List<Blog>> GetPostsAsync();

    // get post by Id
    Task<Blog> GetPostByIdAsync(int Id);

    // save a post
    Task<Blog> SavePostAsync(Blog blog);

    //Delete post
    Task DeletePostAsync(int Id);

    //Update post
    Task<Blog> UpdatePostAsync(int Id, PostViewModel postViewModel);


}

