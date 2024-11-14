using ProductProvider.Business.Models;

namespace ProductProvider.Business.Interfaces;

public interface IProductService
{
    public ProductServiceResponse CreateProduct(ProductRequest productRequest);
    public ProductServiceResponse GetAllProducts();   
    public ProductServiceResponse GetProduct(string id);
    public ProductServiceResponse UpdateProduct(string id, Product product);
    public ProductServiceResponse DeleteProduct(string id);

}
