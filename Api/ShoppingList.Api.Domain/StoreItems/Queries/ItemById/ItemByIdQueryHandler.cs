using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemById
{
    public class ItemByIdQueryHandler : IQueryHandler<ItemByIdQuery, StoreItemReadModel>
    {
        private readonly IItemRepository itemRepository;
        private readonly IStoreItemReadModelConversionService storeItemReadModelConversionService;

        public ItemByIdQueryHandler(IItemRepository itemRepository,
            IStoreItemReadModelConversionService storeItemReadModelConversionService)
        {
            this.itemRepository = itemRepository;
            this.storeItemReadModelConversionService = storeItemReadModelConversionService;
        }

        public async Task<StoreItemReadModel> HandleAsync(ItemByIdQuery query, CancellationToken cancellationToken)
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            cancellationToken.ThrowIfCancellationRequested();

            var item = await itemRepository.FindByAsync(query.ItemId, cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(query.ItemId));

            cancellationToken.ThrowIfCancellationRequested();

            return await storeItemReadModelConversionService.ConvertAsync(item, cancellationToken);
        }
    }
}