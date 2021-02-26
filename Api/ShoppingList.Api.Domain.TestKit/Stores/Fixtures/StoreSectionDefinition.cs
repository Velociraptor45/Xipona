using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ShoppingList.Api.Domain.TestKit.Stores.Fixtures
{
    public class StoreSectionDefinition
    {
        public StoreSectionId Id { get; set; }
        public string Name { get; set; }
        public int? SortingIndex { get; set; }
        public bool? IsDefaultSection { get; set; }

        public static StoreSectionDefinition FromIsDefaultSection(bool isDefaultSection)
        {
            return new StoreSectionDefinition()
            {
                IsDefaultSection = isDefaultSection
            };
        }

        public StoreSectionDefinition Clone()
        {
            return new StoreSectionDefinition()
            {
                Id = Id == null ? null : new StoreSectionId(Id.Value),
                Name = Name,
                SortingIndex = SortingIndex,
                IsDefaultSection = IsDefaultSection
            };
        }
    }
}