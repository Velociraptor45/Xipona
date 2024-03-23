namespace ProjectHermes.Xipona.Api.Domain.Items.Models;

public sealed record Comment(string Value)
{
    public static Comment Empty => new(string.Empty);

    public override string ToString()
    {
        return Value;
    }
}