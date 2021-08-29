using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common;
using System;
using System.Collections.Generic;
using System.Linq;

using DomainModels = ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models
{
    public class ShoppingListBuilder : DomainTestBuilderBase<DomainModels.ShoppingList>
    {
        public ShoppingListBuilder WithId(ShoppingListId id)
        {
            FillContructorWith("id", id);
            return this;
        }

        public ShoppingListBuilder WithStoreId(StoreId storeId)
        {
            FillContructorWith("storeId", storeId);
            return this;
        }

        public ShoppingListBuilder WithCompletionDate(DateTime? completionDate)
        {
            FillContructorWith("completionDate", completionDate);
            return this;
        }

        public ShoppingListBuilder WithoutCompletionDate()
        {
            return WithCompletionDate(null);
        }

        public ShoppingListBuilder WithSections(IEnumerable<IShoppingListSection> sections)
        {
            FillContructorWith("sections", sections);
            return this;
        }

        public ShoppingListBuilder WithSection(IShoppingListSection section)
        {
            return WithSections(section.ToMonoList());
        }

        public ShoppingListBuilder WithoutSections()
        {
            return WithSections(Enumerable.Empty<IShoppingListSection>());
        }
    }
}