using SCISalesTest.Domain.Constants;

namespace SCISalesTest.Domain.Options;

public class ExchangeRateOptions
{
    public const string Key = "ExchangeRateApi";

    public string BaseAddress { get; set; } = string.Empty;
    public string DefaultBaseCurrency { get; set; } = AppConstants.DEFAULT_CURRENCY;
}
