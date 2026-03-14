using Moq;
using SCISalesTest.Application.ApplicationServices;
using SCISalesTest.Application.DTOs.Products;
using SCISalesTest.Application.Feature.Products.GetProducts;
using SCISalesTest.Application.UnitTest.Builders;
using Xunit;

namespace SCISalesTest.Application.UnitTest.Features.Products;

public class GetProductsHandlerTests
{
    private readonly Mock<ProductService> _productServiceMock;
    private readonly GetProductsHandler _handler;

    public GetProductsHandlerTests()
    {
        _productServiceMock = new Mock<ProductService>(
            MockBehavior.Strict,
            null!, null!, null!, null!);

        _handler = new GetProductsHandler(_productServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllProducts()
    {
        // Arrange
        var products = new List<ProductDto>
        {
            new ProductDtoBuilder()
                .WithId(1)
                .WithName("Product A")
                .WithDescription("Desc A")
                .WithPrice(10m)
                .Build(),
            new ProductDtoBuilder()
                .WithId(2)
                .WithName("Product B")
                .WithDescription("Desc B")
                .WithPrice(20m)
                .Build()
        };

        _productServiceMock
            .Setup(s => s.GetAllProductsAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Equal("Product A", resultList[0].Name);
    }

    [Fact]
    public async Task Handle_EmptyList_ReturnsEmptyCollection()
    {
        // Arrange
        _productServiceMock
            .Setup(s => s.GetAllProductsAsync())
            .ReturnsAsync([]);

        // Act
        var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
