namespace Xipona.Frontend.Redux.Items.States.Merges;

public record ItemMergeValidationResult(string? Name, IReadOnlyDictionary<Guid, string> TypeNames)
{
    public ItemMergeValidationResult() : this(null, new Dictionary<Guid, string>(0))
    {
    }

    public bool HasErrors => Name is not null || TypeNames.Any();
}