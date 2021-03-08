using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
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
        private readonly ShoppingListStoreFixture shoppingListStoreFixture;

        private readonly CommonFixture commonFixture;

        public ShoppingListFixture(ShoppingListSectionFixture shoppingListSectionFixture, CommonFixture commonFixture)
        {
            this.shoppingListSectionFixture = shoppingListSectionFixture;
            shoppingListStoreFixture = new ShoppingListStoreFixture(commonFixture);
            this.commonFixture = commonFixture;
        }

        public IShoppingList Create()
        {
            var id = commonFixture.NextInt();
            return Create(new ShoppingListId(id));
        }

        public IShoppingList Create(ShoppingListId id)
        {
            IEnumerable<IShoppingListSection> sections = shoppingListSectionFixture.CreateManyValid();

            var definition = new ShoppingListDefinition
            {
                Id = id,
                Sections = sections
            };

            return Create(definition);
        }

        public IShoppingList Create(ShoppingListDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<ListModels.ShoppingList, ShoppingListId>("id", definition.Id);
            if (definition.Store != null)
                fixture.ConstructorArgumentFor<ListModels.ShoppingList, IShoppingListStore>("store", definition.Store);
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
            baseDefinition.Store ??= shoppingListStoreFixture.CreateValid();
            baseDefinition.Sections ??= shoppingListSectionFixture.CreateManyValid();
            baseDefinition.CompletionDate = null;

            return Create(baseDefinition);
        }
    }
}