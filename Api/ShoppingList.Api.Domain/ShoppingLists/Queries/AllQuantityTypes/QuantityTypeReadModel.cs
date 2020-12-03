namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes
{
    public class QuantityTypeReadModel
    {
        public QuantityTypeReadModel(int id, string name, int defaultQuantity, string pricelabel)
        {
            Id = id;
            Name = name;
            DefaultQuantity = defaultQuantity;
            Pricelabel = pricelabel;
        }

        public int Id { get; }
        public string Name { get; }
        public int DefaultQuantity { get; }
        public string Pricelabel { get; }
    }
}