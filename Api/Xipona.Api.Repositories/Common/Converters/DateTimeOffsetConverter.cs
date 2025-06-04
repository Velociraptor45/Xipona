using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProjectHermes.Xipona.Api.Repositories.Common.Converters;

internal class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
{
    public DateTimeOffsetConverter()
        : base(
            d => d.ToUniversalTime(),
            d => d.ToUniversalTime())
    {
    }
}