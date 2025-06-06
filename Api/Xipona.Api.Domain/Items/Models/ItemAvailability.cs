using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Items.Models;

public sealed record ItemAvailability(StoreId StoreId, Price Price, SectionId DefaultSectionId)
{
    public ItemAvailability TransferToDefaultSection(SectionId sectionId)
    {
        return this with { DefaultSectionId = sectionId };
    }
}