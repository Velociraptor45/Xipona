namespace ProjectHermes.Xipona.Api.Domain.Users.Models;

public readonly record struct UserId
{
    public UserId()
    {
        throw new NotSupportedException("Use the other ctor to create a user id.");
    }

    public UserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(UserId userId)
    {
        return userId.Value;
    }

    public override string ToString()
    {
        return Value.ToString("D");
    }
}