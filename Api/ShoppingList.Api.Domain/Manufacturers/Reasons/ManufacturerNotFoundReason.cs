using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Reasons;

public class ManufacturerNotFoundReason : IReason
{
    public ManufacturerNotFoundReason(ManufacturerId id)
    {
        Message = $"Manufacturer {id.Value} not found.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ManufacturerNotFound;
}