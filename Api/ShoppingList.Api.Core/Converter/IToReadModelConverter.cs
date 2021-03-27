using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Core.Converter
{
    public interface IToReadModelConverter<in TSource, out TDestination>
    {
        TDestination ToReadModel(TSource source);

        IEnumerable<TDestination> ToReadModel(IEnumerable<TSource> sources)
        {
            var sourcesList = sources.ToList();

            foreach (var source in sourcesList)
            {
                yield return ToReadModel(source);
            }
        }
    }
}