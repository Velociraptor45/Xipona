using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;

public class ShoppingListReadModel
{
    private readonly IEnumerable<ShoppingListSectionReadModel> _sections;

    public ShoppingListReadModel(ShoppingListId id, DateTimeOffset? completionDate, ShoppingListStoreReadModel store,
        IEnumerable<ShoppingListSectionReadModel> sections)
    {
        Id = id;
        CompletionDate = completionDate;
        Store = store;
        _sections = sections;
    }

    public ShoppingListId Id { get; }
    public DateTimeOffset? CompletionDate { get; }
    public ShoppingListStoreReadModel Store { get; }
    public IReadOnlyCollection<ShoppingListSectionReadModel> Sections => _sections.ToList().AsReadOnly();
}