namespace ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

public abstract record Name
{
    protected Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("A name's 'Value' cannot be null or whitespace.", nameof(value));

        Value = value;
    }

    public string Value { get; }

    public sealed override string ToString()
    {
        return Value;
    }

    public static implicit operator string(Name name)
    {
        return name.Value;
    }
}