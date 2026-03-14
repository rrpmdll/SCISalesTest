using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCISalesTest.Application.DTOs.ExternalServices;
using SCISalesTest.Application.Feature.CurrencyConversion;

namespace SCISalesTest.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("convert")]
    [Produces(typeof(CurrencyConversionDto))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ConvertAsync(
        [FromQuery] decimal amount,
        [FromQuery] string sourceCurrency,
        [FromQuery] string targetCurrency)
        => Ok(await _mediator.Send(new ConvertCurrencyQuery
        {
            Amount = amount,
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency
        }));

    [HttpGet("supported-currencies")]
    [Produces(typeof(IEnumerable<string>))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetSupportedCurrenciesAsync()
        => Ok(await _mediator.Send(new GetSupportedCurrenciesQuery()));
}
