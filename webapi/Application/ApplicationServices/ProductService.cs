using AutoMapper;
using SCISalesTest.Application.DTOs.Products;
using SCISalesTest.Application.ExternalServices;
using SCISalesTest.Domain.Constants;
using SCISalesTest.Domain.Entities;
using SCISalesTest.Domain.Exceptions;
using SCISalesTest.Domain.Repositories;
using SCISalesTest.Domain.Resources;

namespace SCISalesTest.Application.ApplicationServices;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IMapper _mapper;
    private readonly IMessagesProvider _messagesProvider;

    public ProductService(
        IProductRepository productRepository,
        IExchangeRateService exchangeRateService,
        IMapper mapper,
        IMessagesProvider messagesProvider)
    {
        _productRepository = productRepository;
        _exchangeRateService = exchangeRateService;
        _mapper = mapper;
        _messagesProvider = messagesProvider;
    }

    public virtual async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
    {
        var product = _mapper.Map<Product>(createDto);
        var createdProduct = await _productRepository.CreateAsync(product);
        return _mapper.Map<ProductDto>(createdProduct);
    }

    public virtual async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public virtual async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(string.Format(_messagesProvider.ProductNotFound, id));

        return _mapper.Map<ProductDto>(product);
    }

    public virtual async Task<ProductDto> UpdateProductAsync(UpdateProductDto updateDto)
    {
        _ = await _productRepository.GetByIdAsync(updateDto.Id)
            ?? throw new NotFoundException(string.Format(_messagesProvider.ProductNotFound, updateDto.Id));

        var product = _mapper.Map<Product>(updateDto);
        var updatedProduct = await _productRepository.UpdateAsync(product)
            ?? throw new NotFoundException(string.Format(_messagesProvider.ProductNotFound, updateDto.Id));

        return _mapper.Map<ProductDto>(updatedProduct);
    }

    public virtual async Task<bool> DeleteProductAsync(int id)
    {
        _ = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(string.Format(_messagesProvider.ProductNotFound, id));

        return await _productRepository.DeleteAsync(id);
    }

    public virtual async Task<ProductWithExchangeRateDto> GetProductWithExchangeRateAsync(int productId, string targetCurrency)
    {
        var product = await _productRepository.GetByIdAsync(productId)
            ?? throw new NotFoundException(string.Format(_messagesProvider.ProductNotFound, productId));

        var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(
            AppConstants.DEFAULT_CURRENCY, targetCurrency);

        var dto = _mapper.Map<ProductWithExchangeRateDto>(product);
        dto.OriginalCurrency = AppConstants.DEFAULT_CURRENCY;
        dto.TargetCurrency = targetCurrency.ToUpperInvariant();
        dto.ExchangeRate = exchangeRate;
        dto.ConvertedPrice = Math.Round(product.Price * exchangeRate, 2);

        return dto;
    }
}
