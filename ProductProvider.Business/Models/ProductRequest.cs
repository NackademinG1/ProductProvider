using System.ComponentModel.DataAnnotations;

namespace ProductProvider.Business.Models;

public class ProductRequest
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public decimal Price { get; set; }

    public string? CategoryName { get; set; }
    public int Amount { get; set; }
}
