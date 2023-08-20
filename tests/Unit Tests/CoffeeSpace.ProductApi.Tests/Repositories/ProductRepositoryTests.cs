using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Repositories;
using CoffeeSpace.ProductApi.Persistence.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.ProductApi.Tests.Repositories;

public sealed class ProductRepositoryTests
{
    private readonly IProductDbContext _dbContext;
    private readonly DbSet<Product> _productDbSet;
    private readonly Fixture _fixture;
    private readonly IEnumerable<Product> _products;

    private readonly ProductRepository _productRepository;
    
    public ProductRepositoryTests()
    {
        _fixture = new Fixture();
        _products = _fixture.CreateMany<Product>();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _productDbSet = _products.AsQueryable().BuildMockDbSet();
        _dbContext = _fixture.Create<IProductDbContext>();
        _dbContext.Products.Returns(_productDbSet);
        
        _productRepository = new ProductRepository(_dbContext);
    }

    [Fact]
    public async Task GetCountAsync_ShouldReturnCorrectValue()
    {
        // Arrange
        int expectedCount = _products.Count();
        
        // Act
        int result = await _productRepository.GetCountAsync(CancellationToken.None);

        // Assert   
        result.Should().Be(expectedCount);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        
        // Act
        var result = await _productRepository.GetAllProductsAsync(CancellationToken.None);

        // Assert   
        result.Should().BeEquivalentTo(_products);
    }
    
    [Fact]
    public async Task GetAllWithRequestAsync_ShouldReturnPagedProducts()
    {
        // Arrange
        
        // Act
        var result = await _productRepository.GetAllProductsAsync(CancellationToken.None);

        // Assert   
        result.Should().BeEquivalentTo(_products);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var productToFind = _products.First();
        
        _productDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .Returns(productToFind);
        
        // Act
        var result = await _productRepository.GetProductByIdAsync(productToFind.Id, CancellationToken.None);

        // Assert   
        result.Should().BeEquivalentTo(productToFind);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        string productId = string.Empty;
        
        // Act
        var result = await _productRepository.GetProductByIdAsync(productId, CancellationToken.None);

        // Assert   
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenProductWasCreated()
    {
        // Arrange
        var productToCreate = _fixture.Create<Product>();
        
        _dbContext.SaveChangesAsync()
            .ReturnsForAnyArgs(1);
        
        // Act
        bool result = await _productRepository.CreateProductAsync(productToCreate, CancellationToken.None);

        // Assert   
        result.Should().BeTrue();

        await _productDbSet.Received().AddAsync(productToCreate, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenProductWasNotCreated()
    {
        // Arrange
        var productToCreate = _fixture.Create<Product>();

        _dbContext.SaveChangesAsync()
            .ReturnsForAnyArgs(0);
        
        // Act
        bool result = await _productRepository.CreateProductAsync(productToCreate, CancellationToken.None);

        // Assert   
        result.Should().BeFalse();

        await _productDbSet.Received().AddAsync(productToCreate, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedProduct_WhenProductWasUpdated()
    {
        // Arrange
        var productToUpdate = _fixture.Create<Product>();
        var updatedProduct = _fixture.Build<Product>()
            .With(product => product.Id, productToUpdate.Id)
            .Create();

        _productDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .Returns(productToUpdate);
        
        _dbContext.SaveChangesAsync()
            .ReturnsForAnyArgs(1);
        
        // Act
        var result = await _productRepository.UpdateProductAsync(updatedProduct, CancellationToken.None);

        // Assert   
        result.Should().BeEquivalentTo(updatedProduct);
        _productDbSet.Should().NotContainEquivalentOf(productToUpdate);

        _productDbSet.Received().Update(updatedProduct);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenProductWasNotUpdated()
    {
        // Arrange
        var updatedProduct = _fixture.Create<Product>();

        _productDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        _dbContext.SaveChangesAsync()
            .ReturnsForAnyArgs(0);
        
        // Act
        var result = await _productRepository.UpdateProductAsync(updatedProduct, CancellationToken.None);

        // Assert   
        result.Should().BeNull();
    }
}