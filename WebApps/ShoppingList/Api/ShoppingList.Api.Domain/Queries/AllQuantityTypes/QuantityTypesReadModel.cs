using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.Queries.AllQuantityTypes
{
    public class QuantityTypesReadModel
    {
        private readonly IEnumerable<string> quantityTypes;

        public QuantityTypesReadModel(IEnumerable<string> quantityTypes)
        {
            this.quantityTypes = quantityTypes;
        }

        public IReadOnlyCollection<string> QuantityTypes => quantityTypes.ToList().AsReadOnly();
    }
}