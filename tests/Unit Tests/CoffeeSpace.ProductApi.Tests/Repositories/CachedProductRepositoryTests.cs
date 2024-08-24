using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Repositories;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using CoffeeSpace.Shared.Services.Abstractions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.ProductApi.Tests.Repositories;

public sealed class CachedProductRepositoryTests
{
    private readonly ICacheService _cacheService;
    private readonly IProductRepository _productRepository;
    private readonly IEnumerable<Product> _products;
    private readonly Fixture _fixture;

    private readonly CachedProductRepository _cachedProductRepository;

    public CachedProductRepositoryTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _cacheService = _fixture.Create<ICacheService>();
        _productRepository = _fixture.Create<IProductRepository>();
        _products = _fixture.CreateMany<Product>();

        _cachedProductRepository = new CachedProductRepository(_productRepository, _cacheService);
    }

    [Fact]
    public async Task GetCountAsync_ShouldReturnCorrectValue()
    {
        // Arrange
        int expectedCount = _products.Count();

        _productRepository.GetCountAsync(Arg.Any<CancellationToken>())
            .Returns(expectedCount);

        // Act
        int result = await _cachedProductRepository.GetCountAsync(CancellationToken.None);

        // Assert
        result.Should().Be(expectedCount);

        await _productRepository.Received().GetCountAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts_AndCacheTheValue()
    {
        // Arrange
        _productRepository.GetAllProductsAsync(Arg.Any<CancellationToken>())
            .Returns(_products);

        _cacheService.GetAllOrCreateAsync(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Product>>>>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => callInfo.Arg<Func<Task<IEnumerable<Product>>>>().Invoke());

        // Act
        var result = await _cachedProductRepository.GetAllProductsAsync(CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(_products);

        await _productRepository.Received().GetAllProductsAsync(Arg.Any<CancellationToken>());
        await _cacheService.Received().GetAllOrCreateAsync(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Product>>>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCachedProducts()
    {
        // Arrange
        _cacheService.GetAllOrCreateAsync(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Product>>>>(),Arg.Any<CancellationToken>())
            .Returns(_products);

        // Act
        var result = await _cachedProductRepository.GetAllProductsAsync(CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(_products);

        await _cacheService.Received().GetAllOrCreateAsync(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Product>>>>(), Arg.Any<CancellationToken>());
        await _productRepository.DidNotReceive().GetAllProductsAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_AndCacheTheValue()
    {
        // Arrange
        var expectedProduct = _products.First();
        
        _cacheService.GetOrCreateAsync(Arg.Any<string>(), Arg.Any<Func<Task<Product>>>()!, Arg.Any<CancellationToken>())!
            .Returns(callInfo => callInfo.Arg<Func<Task<Product>>>().Invoke());

        _productRepository.GetProductByIdAsync(expectedProduct.Id, Arg.Any<CancellationToken>())
            .Returns(expectedProduct);

        // Act
        var result = await _cachedProductRepository.GetProductByIdAsync(expectedProduct.Id, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedProduct);

        await _cacheService.Received().GetOrCreateAsync(Arg.Any<string>(), Arg.Any<Func<Task<Product>>>()!, Arg.Any<CancellationToken>());
        await _productRepository.Received().GetProductByIdAsync(expectedProduct.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCachedProduct()
    {
        // Arrange
        var expectedProduct = _products.First();
        _cacheService.GetOrCreateAsync(Arg.Any<string>(), Arg.Any<Func<Task<Product>>>()!, Arg.Any<CancellationToken>())
            .Returns(expectedProduct);

        // Act
        var result = await _cachedProductRepository.GetProductByIdAsync(expectedProduct.Id, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedProduct);

        await _cacheService.Received().GetOrCreateAsync(Arg.Any<string>(), Arg.Any<Func<Task<Product>>>()!, Arg.Any<CancellationToken>());
        await _productRepository.DidNotReceive().GetProductByIdAsync(expectedProduct.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenProductWasCreated_AndResetCacheValue()
    {
        // Arrange
        var productToCreate = _fixture.Create<Product>();
        _productRepository.CreateProductAsync(productToCreate, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        bool result = await _cachedProductRepository.CreateProductAsync(productToCreate, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        
        await _cacheService.Received().RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _productRepository.Received().CreateProductAsync(productToCreate, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenProductWasNotCreated_AndKeepCacheValue()
    {
        // Arrange
        var productToCreate = _fixture.Create<Product>();
        _productRepository.CreateProductAsync(productToCreate, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        bool result = await _cachedProductRepository.CreateProductAsync(productToCreate, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        
        await _cacheService.DidNotReceive().RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _productRepository.Received().CreateProductAsync(productToCreate, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedProduct_WhenProductWasUpdated_AndResetCacheValue()
    {
        // Arrange
        var productToUpdate = _products.First();
        var updatedProduct = _fixture.Build<Product>()
            .With(product => product.Id, productToUpdate.Id)
            .Create();

        _productRepository.UpdateProductAsync(updatedProduct, Arg.Any<CancellationToken>())
            .Returns(updatedProduct);

        // Act
        var result = await _cachedProductRepository.UpdateProductAsync(updatedProduct, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(updatedProduct);
        
        await _cacheService.Received().RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _productRepository.Received().UpdateProductAsync(updatedProduct, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenProductWasNotUpdated_AndKeepCacheValue()
    {
        // Arrange
        var productToUpdate = _products.First();
        var updatedProduct = _fixture.Build<Product>()
            .With(product => product.Id, productToUpdate.Id)
            .Create();

        _productRepository.UpdateProductAsync(updatedProduct, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _cachedProductRepository.UpdateProductAsync(updatedProduct, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        
        await _cacheService.DidNotReceive().RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _productRepository.Received().UpdateProductAsync(updatedProduct, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenProductWasDeleted_AndResetCacheValue()
    {
        // Arrange
        var productToDelete = _products.First();

        _productRepository.DeleteProductByIdAsync(productToDelete.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        bool result = await _cachedProductRepository.DeleteProductByIdAsync(productToDelete.Id, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        
        await _cacheService.Received().RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _productRepository.Received().DeleteProductByIdAsync(productToDelete.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenProductWasNotDeleted_AndKeepCacheValue()
    {
        // Arrange
        var productToDelete = _products.First();

        _productRepository.DeleteProductByIdAsync(productToDelete.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        bool result = await _cachedProductRepository.DeleteProductByIdAsync(productToDelete.Id, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        
        await _cacheService.DidNotReceive().RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _productRepository.Received().DeleteProductByIdAsync(productToDelete.Id, Arg.Any<CancellationToken>());
    }
}