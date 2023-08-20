using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using CoffeeSpace.ProductApi.Application.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.ProductApi.Tests.Services;

public sealed class ProductServiceTests
{
    private readonly IProductRepository _productRepository;
    private readonly Fixture _fixture;
    private readonly IEnumerable<Product> _products;

    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _productRepository = _fixture.Create<IProductRepository>();
        _products = _fixture.Build<Product>()
            .With(product => product.Id, Guid.NewGuid().ToString())
            .CreateMany();

        _productService = new ProductService(_productRepository);
    }

    [Fact]
    public async Task GetCountAsync_ShouldReturnCorrectCount()
    {
        // Arrange
        int expectedCount = _products.Count();
        _productRepository.GetCountAsync(Arg.Any<CancellationToken>())
            .Returns(expectedCount);
        
        // Act
        int result = await _productService.GetCountAsync(CancellationToken.None);

        // Assert
        result.Should().Be(expectedCount);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var request = new GetAllProductsRequest
        {
            Page = 1,
            PageSize = 2
        };
        var expectedProducts = _products.OrderBy(order => order.Title).Take(request.PageSize).ToArray();
        
        _productRepository.GetAllProductsAsync(request, CancellationToken.None)
            .Returns(expectedProducts);
        
        // Act
        var result = await _productService.GetAllProductsAsync(request, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedProducts);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var expectedProduct = _products.First();
        
        _productRepository.GetProductByIdAsync(expectedProduct.Id, Arg.Any<CancellationToken>())
            .Returns(expectedProduct);
        
        // Act
        var result = await _productService.GetProductByIdAsync(Guid.Parse(expectedProduct.Id), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedProduct);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesExist()
    {
        // Arrange
        var emptyId = Guid.Empty;
        
        _productRepository.GetProductByIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = await _productService.GetProductByIdAsync(emptyId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenProductWasCreated()
    {
        // Arrange
        var productToCreate = _fixture.Create<Product>();
        
        _productRepository.CreateProductAsync(productToCreate, Arg.Any<CancellationToken>())
            .Returns(true);
        
        // Act
        bool result = await _productService.CreateProductAsync(productToCreate, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenProductWasNotCreated()
    {
        // Arrange
        var productToCreate = _fixture.Create<Product>();
        
        _productRepository.CreateProductAsync(productToCreate, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // Act
        bool result = await _productService.CreateProductAsync(productToCreate, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedProduct_WhenProductWasUpdated()
    {
        // Arrange
        var productToUpdate = _products.First();
        var updatedProduct = _fixture.Build<Product>()
            .With(product => product.Id, productToUpdate.Id)
            .Create();
        
        _productRepository.UpdateProductAsync(updatedProduct, Arg.Any<CancellationToken>())
            .Returns(updatedProduct);
        
        // Act
        var result = await _productService.UpdateProductAsync(updatedProduct, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(updatedProduct);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenProductWasNotUpdated()
    {
        // Arrange
        var productToUpdate = _products.First();
        
        _productRepository.UpdateProductAsync(productToUpdate, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = await _productService.UpdateProductAsync(productToUpdate, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenProductWasDeleted()
    {
        // Arrange
        var productToDelete = _products.First();
        
        _productRepository.DeleteProductByIdAsync(productToDelete.Id, Arg.Any<CancellationToken>())
            .Returns(true);
        
        // Act
        bool result = await _productService.DeleteProductByIdAsync(Guid.Parse(productToDelete.Id), CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenProductWasNotDeleted()
    {
        // Arrange
        var productToDelete = _products.First();
        
        _productRepository.DeleteProductByIdAsync(productToDelete.Id, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // Act
        bool result = await _productService.DeleteProductByIdAsync(Guid.Parse(productToDelete.Id), CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
}