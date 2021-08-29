using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common;

namespace ShoppingList.Api.Domain.TestKit.Stores.Models
{
    public class StoreSectionBuilder : DomainTestBuilderBase<StoreSection>
    {
        public StoreSectionBuilder WithId(SectionId id)
        {
            FillContructorWith("id", id);
            return this;
        }

        public StoreSectionBuilder WithSortingIndex(int sortingIndex)
        {
            FillContructorWith("sortingIndex", sortingIndex);
            return this;
        }

        public StoreSectionBuilder WithIsDefaultSection(bool isDefaultSection)
        {
            FillContructorWith("isDefaultSection", isDefaultSection);
            return this;
        }
    }
}