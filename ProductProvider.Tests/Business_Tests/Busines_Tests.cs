using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Moq;
using ProductProvider.Business.Interfaces;
using ProductProvider.Business.Models;
using ProductProvider.Business.Services;
using ProductProvider.Data.Services;

namespace ProductProvider.Tests.Business_Tests;

public class Busines_Tests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly string _id = Guid.NewGuid().ToString();

    public Busines_Tests()
    {
        _mockProductService = new Mock<IProductService>();
    }

    [Fact]
    public void CreateProduct_ShouldReturnTrue_WhenProductIsCreated()
    {
        // Arrange

        var categoryEntity = new CategoryEntity
        {
            Id = _id,
            CategoryName = "CategoryTest",
        };

        var productRequest = new ProductRequest
        {
            Name = "Test",
            Price = 100m,
            CategoryName = categoryEntity.CategoryName,
            Amount = 100,
        };

        _mockProductService.Setup(ps => ps.CreateProduct(It.IsAny<ProductRequest>())).Returns(new ProductServiceResponse
        {
            Success = true,
            ProductResult = new Product
            {
                Id = _id,
                Name = "Test",
            }
        });

        // Act
        var response = _mockProductService.Object.CreateProduct(productRequest);

        // Assert
        Assert.NotNull(response.ProductResult);
        Assert.Equal(_id, response.ProductResult!.Id);
        Assert.Equal("Test", response.ProductResult.Name);
    }

    [Fact]
    public void CreateProduct_ShouldReturnFalse_WhenProductCreationFails()
    {
        // Arrange

        var categoryEntity = new CategoryEntity
        {
            Id = _id,
            CategoryName = "CategoryTest",
        };

        var productRequest = new ProductRequest
        {
            Name = "Test",
            Price = 100m,
            CategoryName = categoryEntity.CategoryName,
            Amount = 100,
        };

        _mockProductService.Setup(ps => ps.CreateProduct(It.IsAny<ProductRequest>())).Returns(new ProductServiceResponse
        {
            Success = false,
            ProductResult = null
        });

        // Act
        var response = _mockProductService.Object.CreateProduct(productRequest);

        // Assert
        Assert.Null(response.ProductResult);
        Assert.False(response.Success);
    }

    [Fact]
    public void CreateProduct_ShouldFail_WhenIncorrectInputData()
    {
        // Arrange

        var categoryEntity = new CategoryEntity
        {
            Id = _id,
            CategoryName = "CategoryTest",
        };

        var productRequest = new ProductRequest
        {
            Name = "Test",
            Price = 100m,
            CategoryName = categoryEntity.CategoryName,
            Amount = 100,
        };

        _mockProductService.Setup(ps => ps.CreateProduct(It.IsAny<ProductRequest>())).Returns(new ProductServiceResponse
        {
            Success = true,
            ProductResult = new Product
            {
                Id = _id,
                Name = "Wrong name",
            }
        });

        // Act
        var response = _mockProductService.Object.CreateProduct(productRequest);

        // Assert
        Assert.NotNull(response.ProductResult);
        Assert.NotEqual("Wrong id", response.ProductResult!.Id);
        Assert.NotEqual("Test", response.ProductResult.Name);
    }

    [Fact]
    public void GetAllProducts_ShouldReturnTrue_WhenFetchingProducts()
    {
        // Arrange
        List<Product> productsList = [];

        var product1 = new Product { Id = _id, Name = "Test1", Price = 100m, CategoryName = "Test1" };
        var product2 = new Product { Id = _id, Name = "Test2", Price = 101m, CategoryName = "Test2" };
        var product3 = new Product { Id = _id, Name = "Test3", Price = 102m, CategoryName = "Test3" };
        productsList.Add(product1);
        productsList.Add(product2);
        productsList.Add(product3);

        _mockProductService.Setup(gp => gp.GetAllProducts()).Returns(new ProductServiceResponse { Success = true, Result = productsList });

        // Act
        var response = _mockProductService.Object.GetAllProducts();
        var products = response.Result as List<Product>;

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(products);
        Assert.Equal(3, products.Count);
    }

    [Fact]
    public void GetAllProducts_ShouldReturnFalse_WhenFetchingProducts()
    {
        // Arrange
        List<Product> productsList = [];

        var product1 = new Product { Id = _id, Name = "Test1", Price = 100m, CategoryName = "Test1" };
        var product2 = new Product { Id = _id, Name = "Test2", Price = 101m, CategoryName = "Test2" };
        productsList.Add(product1);
        productsList.Add(product2);

        _mockProductService.Setup(ps => ps.GetAllProducts()).Returns(new ProductServiceResponse { Success = false, Result = productsList });

        // Act
        var response = _mockProductService.Object.GetAllProducts();
        var products = response.Result as List<Product>;

        // Assert
        Assert.False(response.Success);
        Assert.NotNull(products);
        Assert.NotEqual(3, products.Count);
    }

    [Fact]
    public void GetOneProduct_ShouldReturnTrue_WhenGettingOneProductWithId()
    {
        // Arrange
        var product1 = new Product { Id = "1", Name = "Test1", Price = 123m, CategoryName = "Test1 kategori" };
        var product2 = new Product { Id = "2", Name = "Test2", Price = 123m, CategoryName = "Test2 kategori" };
        _mockProductService.Setup(ps => ps.GetProduct(It.IsAny<string>())).Returns(new ProductServiceResponse { Success = true, ProductResult = product1});

        // Act
        var response = _mockProductService.Object.GetProduct(product1.Id);
        var product = response.ProductResult;

        //Assert
        Assert.NotNull(product);
        Assert.True(response.Success);
        Assert.Equal("1", product1.Id);
        Assert.IsType<Product>(product);
    }

    [Fact]
    public void GetOneProduct_ShouldReturnFalse_WhenTryingToGetOneProductWithId()
    {
        // Arrange
        var product1 = new Product { Id = "1", Name = "Test1", Price = 123m, CategoryName = "Test1 kategori" };
        var product2 = new Product { Id = "2", Name = "Test2", Price = 123m, CategoryName = "Test2 kategori" };
        _mockProductService.Setup(ps => ps.GetProduct(It.IsAny<string>())).Returns(new ProductServiceResponse { Success = false, ProductResult = null });

        // Act
        var response = _mockProductService.Object.GetProduct(product1.Id);
        var product = response.ProductResult;

        //Assert
        Assert.Null(product);
        Assert.False(response.Success);
        Assert.NotEqual("2", product1.Id);
    }

    [Fact]
    public void DeleteProduct_ShouldReturnTrue_WhenProductIsDeletedWithId()
    {
        // Arrange
        var product1 = new Product { Id = "1", Name = "Test1", Price = 123m, CategoryName = "Test1 kategori" };
        var product2 = new Product { Id = "2", Name = "Test2", Price = 123m, CategoryName = "Test2 kategori" };
        _mockProductService.Setup(ps => ps.DeleteProduct(It.IsAny<string>())).Returns(new ProductServiceResponse { Success = true, ProductResult = product1 });

        // Act
        var response = _mockProductService.Object.DeleteProduct(product1.Id);
        var product = response.ProductResult;

        //Assert
        Assert.True(response.Success);
        _mockProductService.Verify(x => x.DeleteProduct(product1.Id), Times.Once());
    }

    [Fact]
    public void DeleteProduct_ShouldReturnFalse_WhenTryingToDeleteProductWithId()
    {
        // Arrange
        var product1 = new Product { Id = "1", Name = "Test1", Price = 123m, CategoryName = "Test1 kategori" };
        var product2 = new Product { Id = "2", Name = "Test2", Price = 123m, CategoryName = "Test2 kategori" };
        _mockProductService.Setup(ps => ps.DeleteProduct(product1.Id)).Throws(new ArgumentException("Product not found"));
        var response = _mockProductService.Object;
        // Act

        //Assert
        Assert.Throws<ArgumentException>(() => response.DeleteProduct(product1.Id));
    }
}
