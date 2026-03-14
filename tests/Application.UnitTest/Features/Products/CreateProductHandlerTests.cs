using AutoMapper;
using Moq;
using SCISalesTest.Application.ApplicationServices;
using SCISalesTest.Application.DTOs.Products;
using SCISalesTest.Application.Feature.Products;
using SCISalesTest.Application.Feature.Products.CreateProduct;
using SCISalesTest.Application.UnitTest.Builders;
using Xunit;

namespace SCISalesTest.Application.UnitTest.Features.Products;

public class CreateProductHandlerTests
{
    private readonly Mock<ProductService> _productServiceMock;
    private readonly IMapper _mapper;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _productServiceMock = new Mock<ProductService>(
            MockBehavior.Strict,
            null!, null!, null!, null!);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductProfile>();
        });
        _mapper = mapperConfig.CreateMapper();

        _handler = new CreateProductHandler(_productServiceMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsProductDto()
    {
        // Arrange
        var command = new CreateProductCommandBuilder()
            .WithName("Keyboard")
            .WithDescription("Wireless keyboard")
            .WithPrice(49.99m)
            .Build();

        var expectedDto = new ProductDtoBuilder()
            .WithName(command.Name)
            .WithDescription(command.Description)
            .WithPrice(command.Price)
            .WithUnitsInStock(command.UnitsInStock)
            .Build();

        _productServiceMock
            .Setup(s => s.CreateProductAsync(It.IsAny<CreateProductDto>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Keyboard", result.Name);
        Assert.Equal(49.99m, result.Price);
    }
}
