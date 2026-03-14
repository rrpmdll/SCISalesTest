using AutoMapper;
using Moq;
using SCISalesTest.Application.ApplicationServices;
using SCISalesTest.Application.ExternalServices;
using SCISalesTest.Application.Feature.Products;
using SCISalesTest.Application.UnitTest.Builders;
using SCISalesTest.Domain.Entities;
using SCISalesTest.Domain.Exceptions;
using SCISalesTest.Domain.Repositories;
using SCISalesTest.Domain.Resources;
using Xunit;

namespace SCISalesTest.Application.UnitTest.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IExchangeRateService> _exchangeRateServiceMock;
    private readonly Mock<IMessagesProvider> _messagesProviderMock;
    private readonly IMapper _mapper;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _exchangeRateServiceMock = new Mock<IExchangeRateService>();
        _messagesProviderMock = new Mock<IMessagesProvider>();

        _messagesProviderMock.Setup(m => m.ProductNotFound)
            .Returns("Product with Id '{0}' was not found.");

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductProfile>();
        });
        _mapper = mapperConfig.CreateMapper();

        _productService = new ProductService(
            _productRepositoryMock.Object,
            _exchangeRateServiceMock.Object,
            _mapper,
            _messagesProviderMock.Object);
    }

    [Fact]
    public async Task CreateProductAsync_ValidDto_ReturnsProductDto()
    {
        // Arrange
        var createDto = new CreateProductDtoBuilder()
            .WithName("Test Product")
            .WithDescription("Test Description")
            .WithPrice(25.99m)
            .Build();

        var createdProduct = new ProductBuilder()
            .WithName(createDto.Name)
            .WithDescription(createDto.Description)
            .WithPrice(createDto.Price)
            .WithUnitsInStock(createDto.UnitsInStock)
            .Build();

        _productRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(createdProduct);

        // Act
        var result = await _productService.CreateProductAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdProduct.Id, result.Id);
        Assert.Equal(createDto.Name, result.Name);
        Assert.Equal(createDto.Price, result.Price);
        _productRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new ProductBuilder()
                .WithId(1)
                .WithName("Product 1")
                .WithDescription("Desc 1")
                .WithPrice(10m)
                .Build(),
            new ProductBuilder()
                .WithId(2)
                .WithName("Product 2")
                .WithDescription("Desc 2")
                .WithPrice(20m)
                .Build()
        };

        _productRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        var productDtos = result.ToList();
        Assert.Equal(2, productDtos.Count);
        Assert.Equal("Product 1", productDtos[0].Name);
        Assert.Equal("Product 2", productDtos[1].Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_ExistingId_ReturnsProduct()
    {
        // Arrange
        var product = new ProductBuilder()
            .WithName("Test Product")
            .WithDescription("Test Description")
            .WithPrice(49.99m)
            .Build();

        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _productService.GetProductByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Product", result.Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _productService.GetProductByIdAsync(999));
    }

    [Fact]
    public async Task UpdateProductAsync_ExistingId_ReturnsUpdatedProduct()
    {
        // Arrange
        var existingProduct = new ProductBuilder()
            .WithName("Old Name")
            .WithDescription("Old Description")
            .WithPrice(10m)
            .Build();

        var updateDto = new UpdateProductDtoBuilder()
            .WithName("New Name")
            .WithDescription("New Description")
            .WithPrice(25m)
            .Build();

        var updatedProduct = new ProductBuilder()
            .WithName("New Name")
            .WithDescription("New Description")
            .WithPrice(25m)
            .WithModifiedDate(DateTime.UtcNow)
            .Build();

        _productRepositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existingProduct);
        _productRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync(updatedProduct);

        // Act
        var result = await _productService.UpdateProductAsync(updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Name", result.Name);
        Assert.Equal(25m, result.Price);
    }

    [Fact]
    public async Task UpdateProductAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var updateDto = new UpdateProductDtoBuilder()
            .WithId(999)
            .WithName("X")
            .WithDescription("X")
            .WithPrice(1m)
            .Build();

        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _productService.UpdateProductAsync(updateDto));
    }

    [Fact]
    public async Task DeleteProductAsync_ExistingId_ReturnsTrue()
    {
        // Arrange
        var product = new ProductBuilder()
            .WithName("P")
            .WithDescription("D")
            .WithPrice(1m)
            .Build();

        _productRepositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);
        _productRepositoryMock.Setup(r => r.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _productService.DeleteProductAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteProductAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _productService.DeleteProductAsync(999));
    }

    [Fact]
    public async Task GetProductWithExchangeRateAsync_ValidData_ReturnsConvertedPrice()
    {
        // Arrange
        var product = new ProductBuilder()
            .WithName("Test Product")
            .WithDescription("Test")
            .WithPrice(100m)
            .Build();

        _productRepositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);
        _exchangeRateServiceMock.Setup(s => s.GetExchangeRateAsync("USD", "COP"))
            .ReturnsAsync(4200m);

        // Act
        var result = await _productService.GetProductWithExchangeRateAsync(1, "COP");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100m, result.OriginalPrice);
        Assert.Equal(420000m, result.ConvertedPrice);
        Assert.Equal("COP", result.TargetCurrency);
        Assert.Equal(4200m, result.ExchangeRate);
    }

    [Fact]
    public async Task GetProductWithExchangeRateAsync_ProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _productService.GetProductWithExchangeRateAsync(999, "COP"));
    }
}
