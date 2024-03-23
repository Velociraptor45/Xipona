using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;

public static class ItemAvailabilityMother
{
    public static ItemAvailabilityBuilder Initial()
    {
        return new ItemAvailabilityBuilder();
    }

    public static ItemAvailabilityBuilder ForStore(StoreId storeId)
    {
        return new ItemAvailabilityBuilder().WithStoreId(storeId);
    }

    public static ItemAvailabilityBuilder ForDefaultSection(SectionId sectionId)
    {
        return new ItemAvailabilityBuilder().WithDefaultSectionId(sectionId);
    }
}