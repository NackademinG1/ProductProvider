using Newtonsoft.Json;
using ProductProvider.Business.Factories;
using ProductProvider.Business.Interfaces;
using ProductProvider.Business.Models;
using ProductProvider.Data.Interfaces;

namespace ProductProvider.Business.Services;

public class ProductService : IProductService
{
    private readonly IFileService _fileService;
    private List<ProductEntity> _productEntities = [];

    public ProductService(IFileService fileService)
    {
        _fileService = fileService;
    }

    public ProductServiceResponse CreateProduct(ProductRequest productRequest)
    {
        GetAllProducts();
        var validator = new ProductValidator();
        var validateResponse = validator.Validate(productRequest);

        if (!validateResponse.Success)
        {
            return new ProductServiceResponse { Success = false, Message = validateResponse.Message };
        }

        var productEntity = ProductFactory.Create(productRequest);
        _productEntities.Add(productEntity);

        var response = SaveToFile();

        if (response != null)
        {
            var product = ProductFactory.Create(productEntity);

            return new ProductServiceResponse { Success = true, ProductResult = product };
        }
        else
        {
            return new ProductServiceResponse { Success = false, ProductResult = null };
        }
    }


    public ProductServiceResponse GetAllProducts()
    {
        List<Product> products = [];

        try
        {
            foreach (var productEntity in _productEntities)
            {
                var product = ProductFactory.Create(productEntity);

                products.Add(product);
            }

            if (products.Count > 0 && products != null)
            {
                return new ProductServiceResponse { Success = true, Result = products };
            }
            else
            {
                return new ProductServiceResponse { Success = false, Result = null };
            }
        }
        catch (Exception ex)
        {
            return new ProductServiceResponse { Success = false, Message = ex.Message };
        }
    }

    public ProductServiceResponse GetProduct(string id)
    {
        GetAllProducts();

        try
        {
            var productEntity = _productEntities.FirstOrDefault(p => p.Id == id);


            if (productEntity == null)
            {
                return new ProductServiceResponse { Success = false };
            }

            Product product = ProductFactory.Create(productEntity);

            return new ProductServiceResponse { Success = true, ProductResult = product };
        }
        catch (Exception ex)
        {
            return new ProductServiceResponse { Success = false, Message = ex.Message };
        }
    }

    public ProductServiceResponse UpdateProduct(string id, Product product)
    {
        GetAllProducts();

        try
        {
            var existingProduct = _productEntities.FirstOrDefault(pe => pe.Id == id);

            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;

                _productEntities.Add(existingProduct);

                var response = SaveToFile();
                if (response != null)
                {
                    return new ProductServiceResponse { Success = true };
                }
                else
                {
                    return new ProductServiceResponse { Success = false };
                }
            }
            else
            {
                return new ProductServiceResponse { Success = false, Message = "" };
            }
        }
        catch (Exception ex)
        {
            return new ProductServiceResponse { Success = false, Message = ex.Message };
        }
    }

    public ProductServiceResponse DeleteProduct(string id)
    {
        GetAllProducts();

        var result = _productEntities.FirstOrDefault(p => p.Id == id);

        if (result != null)
        {
            _productEntities.Remove(result);

            var response = SaveToFile();

            if (response.Success)
            {
                return new ProductServiceResponse { Success = true };
            }
            else
            {
                return new ProductServiceResponse { Success = false, Message = "Something vent wrong when deleting product." };
            }
        }
        else
        {
            return new ProductServiceResponse { Success = false };
        }

    }

    private ProductServiceResponse SaveToFile()
    {
        var content = JsonConvert.SerializeObject(_productEntities, Formatting.Indented);
        var response = _fileService.SaveToFile(content);

        if (response.Success)
        {
            return new ProductServiceResponse { Success = true };
        }
        else
        {
            return new ProductServiceResponse { Success = false, Message = "Failed to save product" };
        }
    }

    private void GetFromFile()
    {
        var response = _fileService.GetFromFile();

        foreach (var item in (List<ProductEntity>)response.Result!)
        {
            _productEntities.Add(item);
        }
    }
}
