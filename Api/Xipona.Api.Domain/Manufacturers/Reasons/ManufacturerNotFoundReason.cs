using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Domain.Manufacturers.Reasons;

public class ManufacturerNotFoundReason : IReason
{
    public ManufacturerNotFoundReason(ManufacturerId id)
    {
        Message = $"Manufacturer {id.Value} not found.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ManufacturerNotFound;
}