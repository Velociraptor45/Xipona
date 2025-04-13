using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Modifications;

public interface IItemModificationService
{
    Task ModifyItemWithTypesAsync(ItemWithTypesModification modification);

    Task Modify(ItemModification modification);

    Task TransferToSectionAsync(SectionId oldSectionId);

    Task RemoveAvailabilitiesForAsync(StoreId storeId);
}