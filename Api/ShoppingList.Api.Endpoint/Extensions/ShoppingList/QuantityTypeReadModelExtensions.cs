using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ShoppingList
{
    public static class QuantityTypeReadModelExtensions
    {
        public static QuantityTypeContract ToContract(this QuantityTypeReadModel readModel)
        {
            return new QuantityTypeContract(readModel.Id, readModel.Name, readModel.DefaultQuantity, readModel.Pricelabel);
        }
    }
}