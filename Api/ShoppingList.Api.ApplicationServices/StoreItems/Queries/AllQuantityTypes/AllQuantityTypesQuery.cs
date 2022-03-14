using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.AllQuantityTypes;

public class AllQuantityTypesQuery : IQuery<IEnumerable<QuantityTypeReadModel>>
{
}