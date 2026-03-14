using AutoMapper;
using MediatR;
using SCISalesTest.Application.DTOs.Products;
using SCISalesTest.Application.ExternalServices;
using SCISalesTest.Domain.Constants;
using SCISalesTest.Domain.Repositories;

namespace SCISalesTest.Application.Feature.Products.GetProductsWithExchangeRate;

public class GetProductsWithExchangeRateHandler 
    : IRequestHandler<GetProductsWithExchangeRateQuery, IEnumerable<ProductWithExchangeRateDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IMapper _mapper;

    public GetProductsWithExchangeRateHandler(
        IProductRepository productRepository,
        IExchangeRateService exchangeRateService,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _exchangeRateService = exchangeRateService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductWithExchangeRateDto>> Handle(
        GetProductsWithExchangeRateQuery request, 
        CancellationToken cancellationToken
    )
    {
        var products = await _productRepository
            .GetAllAsync();
        var exchangeRate = await _exchangeRateService
            .GetExchangeRateAsync(AppConstants.DEFAULT_CURRENCY, request.TargetCurrency);

        var result = new List<ProductWithExchangeRateDto>();
        foreach (var product in products)
        {
            var dto = _mapper.Map<ProductWithExchangeRateDto>(product);
            dto.OriginalCurrency = AppConstants.DEFAULT_CURRENCY;
            dto.TargetCurrency = request.TargetCurrency.ToUpperInvariant();
            dto.ExchangeRate = exchangeRate;
            dto.ConvertedPrice = Math.Round(product.Price * exchangeRate, 2);
            result.Add(dto);
        }

        return result;
    }
}
