using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemById
{
    public class ItemByIdQueryHandler : IQueryHandler<ItemByIdQuery, StoreItemReadModel>
    {
        private readonly IItemRepository itemRepository;

        public ItemByIdQueryHandler(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task<StoreItemReadModel> HandleAsync(ItemByIdQuery query, CancellationToken cancellationToken)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var item = await itemRepository.FindByAsync(query.ItemId, cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(query.ItemId));

            cancellationToken.ThrowIfCancellationRequested();

            return item.ToReadModel();
        }
    }
}