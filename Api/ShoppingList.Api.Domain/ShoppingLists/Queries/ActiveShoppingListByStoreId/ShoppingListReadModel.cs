using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;

public class ShoppingListReadModel
{
    private readonly IEnumerable<ShoppingListSectionReadModel> sections;

    public ShoppingListReadModel(ShoppingListId id, DateTime? completionDate, ShoppingListStoreReadModel store,
        IEnumerable<ShoppingListSectionReadModel> sections)
    {
        Id = id;
        CompletionDate = completionDate;
        Store = store;
        this.sections = sections;
    }

    public ShoppingListId Id { get; }
    public DateTime? CompletionDate { get; }
    public ShoppingListStoreReadModel Store { get; }
    public IReadOnlyCollection<ShoppingListSectionReadModel> Sections => sections.ToList().AsReadOnly();
}