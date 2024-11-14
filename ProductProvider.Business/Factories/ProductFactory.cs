using ProductProvider.Business.Models;

namespace ProductProvider.Business.Factories;

public static class ProductFactory
{
    public static ProductEntity Create(ProductRequest productRequest)
    {
        return new ProductEntity
        {
            Id = Guid.NewGuid().ToString(),
            Name = productRequest.Name,
            Price = productRequest.Price,
            Category = new CategoryEntity
            {
                CategoryName = productRequest.CategoryName!
            }
        };
    }

    public static Product Create(ProductEntity productEntity)
    {
        return new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = productEntity.Name,
            Price = productEntity.Price,
            CategoryName = productEntity.Category.CategoryName,
        };
    }
}
