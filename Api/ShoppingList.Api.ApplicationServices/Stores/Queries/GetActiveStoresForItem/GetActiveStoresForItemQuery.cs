using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.GetActiveStoresForItem;
public record GetActiveStoresForItemQuery : IQuery<IEnumerable<IStore>>;