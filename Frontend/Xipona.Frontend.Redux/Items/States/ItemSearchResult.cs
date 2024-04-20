using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.States;

public class ItemSearchResult : ISearchResult
{
    public ItemSearchResult(Guid id, string name, string manufacturerName)
    {
        Id = id;
        Name = name;
        ManufacturerName = manufacturerName;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ManufacturerName { get; set; }
}