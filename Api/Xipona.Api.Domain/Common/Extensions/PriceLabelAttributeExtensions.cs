using Microsoft.Extensions.Caching.Memory;
using Xipona.Api.Core.Attributes;
using Xipona.Api.Core.Constants;
using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Users.Models;

namespace Xipona.Api.Domain.Common.Extensions;

public static class PriceLabelAttributeExtensions
{
    public static string GetFullLabel(this PriceLabelAttribute attr, IMemoryCache cache)
    {
        if (!cache.TryGetValue(CacheKeys.GeneralSettings, out IGeneralSetting? setting))
            throw new DomainException(new GeneralSettingsNotLoadedReason());

        return setting!.CurrencySymbol + attr.PriceLabel;
    }
}
