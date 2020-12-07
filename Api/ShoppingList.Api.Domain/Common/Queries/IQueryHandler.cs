using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Queries
{
    public interface IQueryHandler<in TQuery, TValue>
        where TQuery : IQuery<TValue>
    {
        Task<TValue> HandleAsync(TQuery query, CancellationToken cancellationToken);
    }
}