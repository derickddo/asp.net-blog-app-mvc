using System;

namespace WebApplication1.Services.FileService;

public interface IFileService
{

    // save a file
    Task<string> SaveFileAsync(IFormFile file);

    //Delete file
    Task DeleteFileAsync(string path);

    //Update file
    Task<string> UpdateFileAsync(string path, IFormFile file);

    //Get file
    Task<byte[]> GetFileAsync(string path);

    //Get file path
    Task<bool> CheckImageChangeAsync(string path, IFormFile file);

}
    