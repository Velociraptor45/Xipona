using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification
{
    public class ItemModificationService : IItemModificationService
    {
        private readonly IItemRepository _itemRepository;
        private readonly CancellationToken _cancellationToken;

        public ItemModificationService(IItemRepository itemRepository,
            CancellationToken cancellationToken)
        {
            _itemRepository = itemRepository;
            _cancellationToken = cancellationToken;
        }

        public async Task ModifyItemWithTypesAsync(ItemWithTypesModification modification)
        {
            var item = await _itemRepository.FindByAsync(modification.Id, _cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(modification.Id));

            //todo itemType validation

            item.Modify(modification);

            //todo update shopping lists with items with availability changed

            await _itemRepository.StoreAsync(item, _cancellationToken);
        }
    }
}