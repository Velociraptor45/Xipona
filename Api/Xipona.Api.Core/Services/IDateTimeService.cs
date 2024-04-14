namespace ProjectHermes.Xipona.Api.Core.Services;

public interface IDateTimeService
{
    DateTimeOffset UtcNow { get; }
}