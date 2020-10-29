using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Contracts.Queries.AllQuantityTypes
{
    public class QuantityTypesContract
    {
        private readonly IEnumerable<string> quantityTypes;

        public QuantityTypesContract(IEnumerable<string> quantityTypes)
        {
            this.quantityTypes = quantityTypes;
        }

        public IReadOnlyCollection<string> QuantityTypes => quantityTypes.ToList().AsReadOnly();
    }
}