using ProductProvider.Business.Interfaces;
using ProductProvider.Business.Models;

namespace ProductProvider.Business.Services;

public class ProductValidator : IProductValidator
{
    public ProductValidatorResponse Validate(ProductRequest productRequest)
    {
        var messages = new List<string>();

        if (string.IsNullOrWhiteSpace(productRequest.Name))
        {
            messages.Add("");
        }
        if (productRequest.Price < 0)
        {
            messages.Add("");
        }
        if (productRequest.Amount < 0)
        {
            messages.Add("");
        }
        if (messages.Count == 0)
        {
            return new ProductValidatorResponse { Success = true, Result = productRequest };
        }

        return new ProductValidatorResponse { Success = false, Message = string.Join(", ", messages) };

    }
}
