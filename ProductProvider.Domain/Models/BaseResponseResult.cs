namespace ProductProvider.Business.Models;

public abstract class BaseResponseResult
{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    public object? Result { get; set; }
    public bool Success { get; set; }
}
