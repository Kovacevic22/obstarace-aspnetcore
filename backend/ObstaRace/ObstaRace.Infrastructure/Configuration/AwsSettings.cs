namespace ObstaRace.Infrastructure.Configuration;

public sealed record AwsSettings
{
    public string BucketName { get; init; } = string.Empty;
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = string.Empty;
}