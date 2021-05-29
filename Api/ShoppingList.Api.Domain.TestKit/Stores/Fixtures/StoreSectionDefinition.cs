using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.Stores.Fixtures
{
    public class StoreSectionDefinition
    {
        public SectionId Id { get; set; }
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

        public static StoreSectionDefinition FromId(SectionId id)
        {
            return new StoreSectionDefinition()
            {
                Id = id
            };
        }

        public StoreSectionDefinition Clone()
        {
            return new StoreSectionDefinition()
            {
                Id = Id == null ? null : new SectionId(Id.Value),
                Name = Name,
                SortingIndex = SortingIndex,
                IsDefaultSection = IsDefaultSection
            };
        }
    }
}