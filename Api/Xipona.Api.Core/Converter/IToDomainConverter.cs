namespace ProjectHermes.Xipona.Api.Core.Converter;

public interface IToDomainConverter : IConverter
{ }

public interface IToDomainConverter<in TSource, out TDestination> : IToDomainConverter
{
    TDestination ToDomain(TSource source);

    IEnumerable<TDestination> ToDomain(IEnumerable<TSource> sources)
    {
        var sourcesList = sources.ToList();

        foreach (var source in sourcesList)
        {
            yield return ToDomain(source);
        }
    }
}