using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Stores.Reasons;

public class CannotModifyDeletedSectionReason : IReason
{
    public CannotModifyDeletedSectionReason(SectionId sectionId)
    {
        Message = $"Cannot modify deleted section ({sectionId.Value})";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyDeletedSection;
}