namespace ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed", Justification = "<Pending>")]
public abstract class Name : IEqualityComparer<Name>, IEquatable<Name>
{
    protected Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("A name's 'Value' cannot be null or whitespace.", nameof(value));

        Value = value;
    }

    public string Value { get; }

    public static bool operator ==(Name left, Name right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Value.Equals(right?.Value);
    }

    public static bool operator !=(Name left, Name right)
    {
        if (left is null)
        {
            return !(right is null);
        }

        return !left.Value.Equals(right?.Value);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Name other && Equals(other);
    }

    public bool Equals(Name? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Value == other.Value;
    }

    public bool Equals(Name? x, Name? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (ReferenceEquals(x, null))
            return false;
        if (ReferenceEquals(y, null))
            return false;
        if (x.GetType() != y.GetType())
            return false;
        return x.Value == y.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public int GetHashCode(Name obj)
    {
        return obj.Value.GetHashCode();
    }
}