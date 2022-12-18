namespace ShoppingList.Frontend.Redux.Shared.Ports.Requests.ItemCategories
{
    public class ModifyItemCategoryRequest : IApiRequest
    {
        public ModifyItemCategoryRequest(Guid itemCategoryId, string name)
        {
            RequestId = Guid.NewGuid();
            ItemCategoryId = itemCategoryId;
            Name = name;
        }

        public Guid ItemCategoryId { get; }
        public string Name { get; }

        public Guid RequestId { get; }
    }
}