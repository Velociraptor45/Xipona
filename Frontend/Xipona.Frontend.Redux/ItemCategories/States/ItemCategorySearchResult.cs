using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;

public class ItemCategorySearchResult : ISearchResult
{
    public ItemCategorySearchResult(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}