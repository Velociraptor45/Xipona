namespace ProjectHermes.Xipona.Api.Core.Services;

public class DateTimeService : IDateTimeService
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}