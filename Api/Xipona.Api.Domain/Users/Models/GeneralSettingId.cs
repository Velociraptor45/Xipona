namespace Xipona.Api.Domain.Users.Models;

public readonly record struct GeneralSettingId(int Value)
{
    public GeneralSettingId() : this(0)
    {
        throw new NotSupportedException("Use the other ctor to create a general setting id.");
    }

    public static implicit operator int(GeneralSettingId generalSettingId)
    {
        return generalSettingId.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
