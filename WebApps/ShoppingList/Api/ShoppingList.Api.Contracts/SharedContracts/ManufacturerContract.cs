namespace ShoppingList.Api.Contracts.SharedContracts
{
    public class ManufacturerContract
    {
        public ManufacturerContract(int id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
        }

        public int Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
    }
}