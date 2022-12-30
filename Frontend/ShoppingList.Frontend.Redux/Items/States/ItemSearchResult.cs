using ProjectHermes.ShoppingList.Frontend.Models.Shared;

namespace ShoppingList.Frontend.Redux.Items.States;

public class ItemSearchResult : ISearchResult
{
    public ItemSearchResult(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}