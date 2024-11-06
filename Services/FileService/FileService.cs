using System;
using WebApplication1.ViewModels;

namespace WebApplication1.Services.FileService;

public class FileService : IFileService
{   
    private readonly IWebHostEnvironment _env;
    
    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }
    public Task DeleteFileAsync(string path)
    {
        // check if the path is null
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        // create a path to the file
        var filePath = Path.Combine(_env.WebRootPath, path);

        // check if the file exists
        if (File.Exists(filePath))
        {
            // delete the file
            File.Delete(filePath);
            return Task.CompletedTask;
        }
        else
        {
            throw new Exception("File not found");
        }
    }

    public Task<byte[]> GetFileAsync(string path)
    {
        // check if the path is null
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        // create a path to the file
        var filePath = Path.Combine(_env.WebRootPath, path);

        // check if the file exists
        if (File.Exists(filePath))
        {
            // read the file
            var file = File.ReadAllBytes(filePath);
            return Task.FromResult(file);
        }
        else
        {
            throw new Exception("File not found");
        }
    }

    public Task<bool> CheckImageChangeAsync(string path, IFormFile file)
    {
        var newFileName = file.FileName;
        var oldFileName = Path.GetFileName(path);

        if (newFileName == oldFileName)
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(true);


    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {   
        // check if the file is null
        if (file == null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        // generate a unique file name
        var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);

        // create a path to save the file
        var filePath = Path.Combine(_env.WebRootPath, "img", fileName);

        // save the file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // return relative path
        return Path.Combine("img", fileName);

    }

    public async Task<string> UpdateFileAsync(string path, IFormFile image) 
    {
        if(string.IsNullOrEmpty(path))
        {
           // upload the image
            var getImagePath = await SaveFileAsync(image);
            return getImagePath;
        }
        else{
            // delete the old image
            await DeleteFileAsync(path);
            
            // upload the new image
            var getImagePath = await SaveFileAsync(image);
            return getImagePath;

        }
    }
}
