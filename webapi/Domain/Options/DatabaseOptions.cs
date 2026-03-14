namespace SCISalesTest.Domain.Options;

public class DatabaseOptions
{
    public const string Key = "ConnectionStrings";

    public string DefaultConnection { get; set; } = string.Empty;
}
