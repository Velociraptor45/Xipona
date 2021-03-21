using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Core.Converter
{
    public interface IToDomainConverter<in TSource, out TDestination>
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
}