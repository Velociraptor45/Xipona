using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System;
using System.Collections.Generic;

using ListModels = ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListFixture
    {
        private readonly ShoppingListSectionFixture shoppingListSectionFixture;
        private readonly CommonFixture commonFixture;

        public ShoppingListFixture(ShoppingListSectionFixture shoppingListSectionFixture, CommonFixture commonFixture)
        {
            this.shoppingListSectionFixture = shoppingListSectionFixture;
            this.commonFixture = commonFixture;
        }

        public IShoppingList Create()
        {
            var id = commonFixture.NextInt();
            return Create(new ShoppingListId(id));
        }

        public IShoppingList Create(ShoppingListId id)
        {
            IEnumerable<IShoppingListSection> sections = shoppingListSectionFixture.CreateMany(3);

            var definition = new ShoppingListGenerationDefinition
            {
                Id = id,
                Sections = sections
            };

            return Create(definition);
        }

        public IShoppingList Create(ShoppingListGenerationDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();
            if (definition.SectionDefinitions != null)
                definition.Sections = shoppingListSectionFixture.CreateMany(definition.SectionDefinitions);

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<ListModels.ShoppingList, ShoppingListId>("id", definition.Id);
            if (definition.Store != null)
                fixture.ConstructorArgumentFor<ListModels.ShoppingList, IShoppingListStore>("store", definition.Store);
            if (definition.Sections != null) //potential problem: duplicated section ids
                fixture.ConstructorArgumentFor<ListModels.ShoppingList, IEnumerable<IShoppingListSection>>("sections", definition.Sections);
            if (definition.UseCompletionDate)
                fixture.ConstructorArgumentFor<ListModels.ShoppingList, DateTime?>("completionDate", definition.CompletionDate);

            return fixture.Create<ListModels.ShoppingList>();
        }

        public IEnumerable<IShoppingList> CreateMany(int count)
        {
            var ids = commonFixture.NextUniqueInts(count);
            foreach (var id in ids)
            {
                yield return Create(new ShoppingListId(id));
            }
        }
    }
}