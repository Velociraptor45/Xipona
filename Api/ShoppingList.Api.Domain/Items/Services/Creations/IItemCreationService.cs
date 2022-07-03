using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;

public interface IItemCreationService
{
    Task<StoreItemReadModel> CreateItemWithTypesAsync(IItem item);

    Task<StoreItemReadModel> CreateAsync(ItemCreation creation);

    Task<StoreItemReadModel> CreateTemporaryAsync(TemporaryItemCreation creation);
}