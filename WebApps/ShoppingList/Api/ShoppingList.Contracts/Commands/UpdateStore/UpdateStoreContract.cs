namespace ShoppingList.Contracts.Commands.UpdateStore
{
    public class UpdateStoreContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}