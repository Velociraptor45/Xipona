﻿using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class CannotCreateItemWithTypesWithoutTypesReason : IReason
{
    public CannotCreateItemWithTypesWithoutTypesReason(ItemId itemId)
    {
        Message = $"Cannot create item with types without types (id: {itemId.Value}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotCreateItemWithTypesWithoutTypes;
}