using System.Resources;
using SCISalesTest.Domain.Resources;

namespace SCISalesTest.Infrastructure.Resources;

public class MessagesProvider : IMessagesProvider
{
    private readonly ResourceManager _resourceManager;

    public MessagesProvider()
    {
        _resourceManager = new ResourceManager("SCISalesTest.Infrastructure.Resources.Messages", typeof(MessagesProvider).Assembly);
    }

    public string ProductNotFound => _resourceManager.GetString("ProductNotFound")!;
    public string ProductNameRequired => _resourceManager.GetString("ProductNameRequired")!;
    public string ProductPriceInvalid => _resourceManager.GetString("ProductPriceInvalid")!;
    public string ProductDescriptionRequired => _resourceManager.GetString("ProductDescriptionRequired")!;
    public string ProductAlreadyExists => _resourceManager.GetString("ProductAlreadyExists")!;
    public string ProductDeleteFailed => _resourceManager.GetString("ProductDeleteFailed")!;
    public string ExchangeRateServiceError => _resourceManager.GetString("ExchangeRateServiceError")!;
    public string ExternalServiceUnavailable => _resourceManager.GetString("ExternalServiceUnavailable")!;
}
