using ProductProvider.Data.Interfaces;
using ProductProvider.Data.Models;

namespace ProductProvider.Data.Services;

public class FileService : IFileService
{
    private readonly string _filePath = "";

    public FileService(string filePath)
    {
        _filePath = filePath;
    }

    public FileServiceResponse SaveToFile(string content)
    {
        try
        {
            using var sw = new StreamWriter(_filePath);
            sw.WriteLine(content);
            return new FileServiceResponse { Success = true };
        }
        catch
        {
            return new FileServiceResponse { Success = false };
        }
    }
    public FileServiceResponse GetFromFile()
    {
        try
        {
            using var sr = new StreamReader(_filePath);
            var data = sr.ReadToEnd();
            return new FileServiceResponse { Success = true, Result = data };
        }
        catch
        {
            return new FileServiceResponse { Success = false, Message = "Something went wrong." };
        }
    }
}
