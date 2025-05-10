using Microsoft.Extensions.Caching.Memory;
using ProjectHermes.Xipona.Api.Core.Attributes;
using ProjectHermes.Xipona.Api.Core.Constants;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Users.Models;

namespace ProjectHermes.Xipona.Api.Domain.Common.Extensions;

public static class PriceLabelAttributeExtensions
{
    public static string GetFullLabel(this PriceLabelAttribute attr, IMemoryCache cache)
    {
        if (!cache.TryGetValue(CacheKeys.GeneralSettings, out IGeneralSetting? setting))
            throw new DomainException(new GeneralSettingsNotLoadedReason());

        return setting!.CurrencySymbol + attr.PriceLabel;
    }
}
