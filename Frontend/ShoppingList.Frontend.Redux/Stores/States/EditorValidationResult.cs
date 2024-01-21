namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;

public record EditorValidationResult(string? Name, IReadOnlyDictionary<Guid, string> SectionNames)
{
    public EditorValidationResult() : this((string?)null, new Dictionary<Guid, string>(0))
    {
    }

    public bool HasErrors => Name is not null || SectionNames.Any();
}