using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCISalesTest.Application.DTOs.Products;
using SCISalesTest.Application.Feature.Products.CreateProduct;
using SCISalesTest.Application.Feature.Products.DeleteProduct;
using SCISalesTest.Application.Feature.Products.GetProductById;
using SCISalesTest.Application.Feature.Products.GetProducts;
using SCISalesTest.Application.Feature.Products.GetProductsWithExchangeRate;
using SCISalesTest.Application.Feature.Products.GetProductWithExchangeRate;
using SCISalesTest.Application.Feature.Products.UpdateProduct;

namespace SCISalesTest.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Produces(typeof(IEnumerable<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await _mediator.Send(new GetProductsQuery()));

    [HttpGet("{id:int}")]
    [Produces(typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)
        => Ok(await _mediator.Send(new GetProductByIdQuery { Id = id }));

    [HttpPost]
    [Produces(typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductCommand command)
    {
        var product = await _mediator.Send(command);
        return CreatedAtAction("GetById", new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    [Produces(typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { Message = "Route id does not match body id." });

        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _mediator.Send(new DeleteProductCommand { Id = id });
        return NoContent();
    }

    [HttpGet("{id:int}/exchange-rate")]
    [Produces(typeof(ProductWithExchangeRateDto))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetWithExchangeRateAsync(int id, [FromQuery] string targetCurrency)
        => Ok(await _mediator.Send(new GetProductWithExchangeRateQuery
        {
            ProductId = id,
            TargetCurrency = targetCurrency
        }));

    [HttpGet("exchange-rate")]
    [Produces(typeof(IEnumerable<ProductWithExchangeRateDto>))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetAllWithExchangeRateAsync([FromQuery] string targetCurrency)
        => Ok(await _mediator.Send(new GetProductsWithExchangeRateQuery
        {
            TargetCurrency = targetCurrency
        }));
}
