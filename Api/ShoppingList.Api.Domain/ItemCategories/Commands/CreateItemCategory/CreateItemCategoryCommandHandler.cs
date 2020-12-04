using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.CreateItemCategory
{
    public class CreateItemCategoryCommandHandler : ICommandHandler<CreateItemCategoryCommand, bool>
    {
        private readonly IItemCategoryRepository itemCategoryRepository;

        public CreateItemCategoryCommandHandler(IItemCategoryRepository itemCategoryRepository)
        {
            this.itemCategoryRepository = itemCategoryRepository;
        }

        public async Task<bool> HandleAsync(CreateItemCategoryCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var model = new ItemCategory(new ItemCategoryId(0), command.Name, false);
            await itemCategoryRepository.StoreAsync(model, cancellationToken);
            return true;
        }
    }
}