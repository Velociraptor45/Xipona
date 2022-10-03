namespace ProjectHermes.ShoppingList.Api.Core.Services;

public interface IDateTimeService
{
    DateTimeOffset UtcNow { get; }
}