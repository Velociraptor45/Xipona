using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Reasons;

public class MultipleDefaultSectionsReason : IReason
{
    public string Message => "Multiple sections are defined as the default section for the store";
    public ErrorReasonCode ErrorCode => ErrorReasonCode.MultipleDefaultSections;
}