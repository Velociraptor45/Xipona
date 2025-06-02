using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;

public class ShoppingListReadModel
{
    public ShoppingListReadModel(ShoppingListId id, DateTimeOffset? completionDate, ShoppingListStoreReadModel store,
        IEnumerable<ShoppingListSectionReadModel> sections, IEnumerable<ListDiscount> listDiscounts)
    {
        Id = id;
        CompletionDate = completionDate;
        Store = store;
        Sections = sections.ToList();
        ListDiscounts = listDiscounts.ToList();
    }

    public ShoppingListId Id { get; }
    public DateTimeOffset? CompletionDate { get; }
    public ShoppingListStoreReadModel Store { get; }
    public IReadOnlyCollection<ShoppingListSectionReadModel> Sections { get; }
    public IReadOnlyCollection<ListDiscount> ListDiscounts { get; }
}