using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common
{
    public interface IToContractConverter : IConverter
    { }

    public interface IToContractConverter<in TSource, out TDestination> : IToContractConverter
    {
        TDestination ToContract(TSource source);

        IEnumerable<TDestination> ToContract(IEnumerable<TSource> sources)
        {
            var sourcesList = sources.ToList();

            foreach (var source in sourcesList)
            {
                yield return ToContract(source);
            }
        }
    }
}