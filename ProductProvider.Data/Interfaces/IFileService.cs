using ProductProvider.Business.Models;
using ProductProvider.Data.Models;

namespace ProductProvider.Data.Interfaces;

public interface IFileService
{
    public FileServiceResponse SaveToFile(string content);
    public FileServiceResponse GetFromFile();
}
