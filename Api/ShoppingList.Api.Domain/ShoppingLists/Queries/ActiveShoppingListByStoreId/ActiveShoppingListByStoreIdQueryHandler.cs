using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId
{
    public class ActiveShoppingListByStoreIdQueryHandler
        : IQueryHandler<ActiveShoppingListByStoreIdQuery, ShoppingListReadModel>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IShoppingListReadModelConversionService shoppingListReadModelConversionService;

        public ActiveShoppingListByStoreIdQueryHandler(IShoppingListRepository shoppingListRepository,
            IShoppingListReadModelConversionService shoppingListReadModelConversionService)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.shoppingListReadModelConversionService = shoppingListReadModelConversionService;
        }

        public async Task<ShoppingListReadModel> HandleAsync(ActiveShoppingListByStoreIdQuery query,
            CancellationToken cancellationToken)
        {
            var shoppingList = await shoppingListRepository.FindActiveByAsync(query.StoreId, cancellationToken);
            if (shoppingList == null)
                throw new DomainException(new ShoppingListNotFoundReason(query.StoreId));

            cancellationToken.ThrowIfCancellationRequested();

            return await shoppingListReadModelConversionService.ConvertAsync(shoppingList, cancellationToken);
        }
    }
}