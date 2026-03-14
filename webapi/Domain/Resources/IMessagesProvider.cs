namespace SCISalesTest.Domain.Resources;

public interface IMessagesProvider
{
    string ProductNotFound { get; }
    string ProductNameRequired { get; }
    string ProductPriceInvalid { get; }
    string ProductDescriptionRequired { get; }
    string ProductAlreadyExists { get; }
    string ProductDeleteFailed { get; }
    string ExchangeRateServiceError { get; }
    string ExternalServiceUnavailable { get; }
}
