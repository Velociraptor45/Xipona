using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemGenerationDefinition
    {
        public StoreItemId Id { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsTemporary { get; set; }

        public static StoreItemGenerationDefinition FromTemporary(bool isTemporary)
        {
            return new StoreItemGenerationDefinition
            {
                IsTemporary = isTemporary
            };
        }
    }
}