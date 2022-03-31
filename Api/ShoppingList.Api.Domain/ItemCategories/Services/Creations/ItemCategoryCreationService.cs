using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Creations;

public class ItemCategoryCreationService : IItemCategoryCreationService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IItemCategoryFactory _itemCategoryFactory;
    private readonly CancellationToken _cancellationToken;

    public ItemCategoryCreationService(
        IItemCategoryRepository itemCategoryRepository,
        IItemCategoryFactory itemCategoryFactory,
        CancellationToken cancellationToken)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _itemCategoryFactory = itemCategoryFactory;
        _cancellationToken = cancellationToken;
    }

    public async Task<IItemCategory> CreateAsync(ItemCategoryName name)
    {
        var model = _itemCategoryFactory.Create(ItemCategoryId.New, name, false);
        return await _itemCategoryRepository.StoreAsync(model, _cancellationToken);
    }
}