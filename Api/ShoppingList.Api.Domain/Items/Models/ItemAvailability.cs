using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public sealed record ItemAvailability(StoreId StoreId, Price Price, SectionId DefaultSectionId)
{
    public ItemAvailability TransferToDefaultSection(SectionId sectionId)
    {
        return new ItemAvailability(
            StoreId,
            Price,
            sectionId);
    }
}