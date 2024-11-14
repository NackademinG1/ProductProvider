using System.ComponentModel.DataAnnotations;

namespace ProductProvider.Business.Models;

public class ProductEntity
{
    [Key]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public CategoryEntity Category { get; set; } = null!;
}
