namespace ProjectHermes.Xipona.Api.Core.Converter;

public interface IToEntityConverter<in TSource, out TDestination>
{
    TDestination ToEntity(TSource source);

    IEnumerable<TDestination> ToEntity(IEnumerable<TSource> sources)
    {
        var sourcesList = sources.ToList();

        foreach (var source in sourcesList)
        {
            yield return ToEntity(source);
        }
    }
}