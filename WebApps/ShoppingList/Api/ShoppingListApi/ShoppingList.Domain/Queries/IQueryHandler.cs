using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Queries
{
    public interface IQueryHandler<in TQuery, TValue>
        where TQuery : IQuery<TValue>
    {
        Task<TValue> HandleAsync(TQuery query, CancellationToken cancellationToken);
    }
}