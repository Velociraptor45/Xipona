namespace ProjectHermes.Xipona.Frontend.Redux.Items.States;
public record EditorValidationResult(string? Name, string? ItemCategory, string? StoreOrTypes, string? NoStores,
    string? NoTypes, IReadOnlyDictionary<Guid, string> TypeNames, IReadOnlyDictionary<Guid, string> NoTypeStores,
    IReadOnlyDictionary<Guid, string> DuplicatedTypeStores, string? DuplicatedStores)
{
    public EditorValidationResult() : this(null, null, null, null, null, new Dictionary<Guid, string>(0),
        new Dictionary<Guid, string>(0), new Dictionary<Guid, string>(0), null)
    {
    }

    public bool HasErrors =>
        Name is not null || ItemCategory is not null || StoreOrTypes is not null || NoStores is not null
        || NoTypes is not null || TypeNames.Any() || NoTypeStores.Any() || DuplicatedTypeStores.Any()
        || DuplicatedStores is not null;
}