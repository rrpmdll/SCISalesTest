using System.Net;
using System.Net.Http.Json;
using Moq;
using SCISalesTest.Application.DTOs.Products;
using SCISalesTest.Domain.Entities;
using SCISalesTest.WebApi.IntegrationTest.Builders;
using SCISalesTest.WebApi.IntegrationTest.Setup;
using Xunit;

namespace SCISalesTest.WebApi.IntegrationTest.Controllers;

public class ProductControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ProductControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new ProductBuilder()
                .WithId(1)
                .WithName("Product A")
                .WithDescription("Desc A")
                .WithPrice(10m)
                .Build(),
            new ProductBuilder()
                .WithId(2)
                .WithName("Product B")
                .WithDescription("Desc B")
                .WithPrice(20m)
                .Build()
        };

        _factory.ProductRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(products);

        // Act
        var response = await _client.GetAsync("/api/product");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetById_ExistingProduct_ReturnsOk()
    {
        // Arrange
        var product = new ProductBuilder()
            .WithName("Test Product")
            .WithDescription("Test Desc")
            .WithPrice(49.99m)
            .Build();

        _factory.ProductRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var response = await _client.GetAsync("/api/product/1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(result);
        Assert.Equal("Test Product", result.Name);
    }

    [Fact]
    public async Task GetById_NonExistingProduct_ReturnsNotFound()
    {
        // Arrange
        _factory.ProductRepositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Product?)null);

        // Act
        var response = await _client.GetAsync("/api/product/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ValidProduct_ReturnsCreated()
    {
        // Arrange
        var command = new CreateProductCommandBuilder()
            .WithName("New Product")
            .WithDescription("New Description")
            .WithPrice(29.99m)
            .Build();

        var createdProduct = new ProductBuilder()
            .WithName(command.Name)
            .WithDescription(command.Description)
            .WithPrice(command.Price)
            .WithUnitsInStock(command.UnitsInStock)
            .Build();

        _factory.ProductRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(createdProduct);

        // Act
        var response = await _client.PostAsJsonAsync("/api/product", command);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingProduct_ReturnsNoContent()
    {
        // Arrange
        var product = new ProductBuilder()
            .WithName("P")
            .WithDescription("D")
            .WithPrice(1m)
            .Build();

        _factory.ProductRepositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);
        _factory.ProductRepositoryMock.Setup(r => r.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var response = await _client.DeleteAsync("/api/product/1");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetWithExchangeRate_ValidData_ReturnsOkWithConvertedPrice()
    {
        // Arrange
        var product = new ProductBuilder()
            .WithName("Product")
            .WithDescription("Desc")
            .WithPrice(100m)
            .Build();

        _factory.ProductRepositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);
        _factory.ExchangeRateServiceMock
            .Setup(s => s.GetExchangeRateAsync("USD", "EUR"))
            .ReturnsAsync(0.92m);

        // Act
        var response = await _client.GetAsync("/api/product/1/exchange-rate?targetCurrency=EUR");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<ProductWithExchangeRateDto>();
        Assert.NotNull(result);
        Assert.Equal(92m, result.ConvertedPrice);
        Assert.Equal("EUR", result.TargetCurrency);
    }
}
