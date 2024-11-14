using ProductProvider.Business.Models;

namespace ProductProvider.Business.Interfaces;

public interface IProductValidator
{
    public ProductValidatorResponse Validate(ProductRequest productRequest);
}
