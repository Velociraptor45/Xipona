﻿using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class CannotUpdateItemTypeWithoutAvailabilitiesReason : IReason
{
    public CannotUpdateItemTypeWithoutAvailabilitiesReason()
    {
        Message = "Cannot update item type without availabilities";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotUpdateItemTypeWithoutAvailabilities;
}