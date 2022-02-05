using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryCommandHandler : ICommandHandler<CreateItemCategoryCommand, bool>
{
    private readonly IItemCategoryRepository itemCategoryRepository;
    private readonly IItemCategoryFactory itemCategoryFactory;

    public CreateItemCategoryCommandHandler(IItemCategoryRepository itemCategoryRepository,
        IItemCategoryFactory itemCategoryFactory)
    {
        this.itemCategoryRepository = itemCategoryRepository;
        this.itemCategoryFactory = itemCategoryFactory;
    }

    public async Task<bool> HandleAsync(CreateItemCategoryCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        var model = itemCategoryFactory.Create(ItemCategoryId.New, command.Name, false);
        await itemCategoryRepository.StoreAsync(model, cancellationToken);
        return true;
    }
}