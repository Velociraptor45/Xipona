namespace Xipona.Api.Core.Services;

public interface IDateTimeService
{
    DateTimeOffset UtcNow { get; }
}