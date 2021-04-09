using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

using ListModels = ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListFixture : IModelFixture<IShoppingList, ShoppingListDefinition>
    {
        private readonly ShoppingListSectionFixture shoppingListSectionFixture;
        private readonly CommonFixture commonFixture;

        public ShoppingListFixture(CommonFixture commonFixture)
        {
            shoppingListSectionFixture = new ShoppingListSectionFixture(commonFixture);
            this.commonFixture = commonFixture;
        }

        public IShoppingList Create(ShoppingListDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<ListModels.ShoppingList, ShoppingListId>("id", definition.Id);
            if (definition.StoreId != null)
                fixture.ConstructorArgumentFor<ListModels.ShoppingList, StoreId>("storeId", definition.StoreId);
            if (definition.Sections != null)
                fixture.ConstructorArgumentFor<ListModels.ShoppingList, IEnumerable<IShoppingListSection>>("sections", definition.Sections);
            if (definition.UseCompletionDate)
                fixture.ConstructorArgumentFor<ListModels.ShoppingList, DateTime?>("completionDate", definition.CompletionDate);

            return fixture.Create<ListModels.ShoppingList>();
        }

        public IEnumerable<IShoppingList> CreateManyValid(int count = 3)
        {
            List<int> uniqueIds = commonFixture.NextUniqueInts(count).ToList();

            foreach (var id in uniqueIds)
            {
                var definition = ShoppingListDefinition.FromId(id);

                yield return Create(definition);
            }
        }

        public IEnumerable<IShoppingList> CreateManyValid(ShoppingListDefinition definition, int count = 3)
        {
            List<int> uniqueIds = commonFixture.NextUniqueInts(count).ToList();

            foreach (var id in uniqueIds)
            {
                var clone = definition.Clone();
                clone.Id = new ShoppingListId(id);

                yield return Create(clone);
            }
        }

        public IEnumerable<IShoppingList> CreateManyValid(IEnumerable<ShoppingListDefinition> definitions)
        {
            var definitionsList = definitions.ToList();
            IEnumerable<int> existingIds = definitionsList
                .Where(d => d.Id != null)
                .Select(d => d.Id.Value);
            List<int> uniqueIds = commonFixture
                .NextUniqueInts(definitionsList.Count, existingIds)
                .ToList();

            for (int i = 0; i < definitionsList.Count; i++)
            {
                int id = uniqueIds[i];
                var definition = definitionsList[i];
                definition.Id ??= new ShoppingListId(id);
                yield return Create(definition);
            }
        }

        public IShoppingList CreateValid(ShoppingListDefinition baseDefinition)
        {
            baseDefinition.Sections ??= shoppingListSectionFixture.CreateManyValid();
            baseDefinition.CompletionDate = null;

            return Create(baseDefinition);
        }

        public IModelFixture<IShoppingList, ShoppingListDefinition> AsModelFixture()
        {
            return this;
        }

        public IShoppingList CreateValidWithOneEmptySection()
        {
            var definitions = new List<ShoppingListSectionDefinition>
            {
                new ShoppingListSectionDefinition(),
                new ShoppingListSectionDefinition
                {
                    Items = Enumerable.Empty<IShoppingListItem>()
                },
                new ShoppingListSectionDefinition()
            };

            var listDefinition = new ShoppingListDefinition
            {
                Sections = shoppingListSectionFixture.CreateManyValid(definitions)
            };

            return CreateValid(listDefinition);
        }
    }
}