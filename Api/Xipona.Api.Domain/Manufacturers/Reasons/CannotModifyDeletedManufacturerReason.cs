using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Domain.Manufacturers.Reasons;

public class CannotModifyDeletedManufacturerReason : IReason
{
    public CannotModifyDeletedManufacturerReason(ManufacturerId id)
    {
        Message = $"Cannot modify manufacturer with id '{id.Value}' because it is deleted.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyDeletedManufacturer;
}