namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public sealed class Comment : IEquatable<Comment>
{
    public Comment(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Value { get; }

    public static Comment Empty => new(String.Empty);

    public bool Equals(Comment? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Comment other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Comment? left, Comment? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Comment? left, Comment? right)
    {
        return !Equals(left, right);
    }
}