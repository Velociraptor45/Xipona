using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.Common.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListStoreFixture : IModelFixture<IShoppingListStore, ShoppingListStoreDefinition>
    {
        private readonly CommonFixture commonFixture;

        public ShoppingListStoreFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public IShoppingListStore Create(ShoppingListStoreDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<ShoppingListStore, ShoppingListStoreId>("id", definition.Id);
            if (definition.Name != null)
                fixture.ConstructorArgumentFor<ShoppingListStore, string>("name", definition.Name);
            if (definition.IsDeleted != null)
                fixture.ConstructorArgumentFor<ShoppingListStore, bool>("isDeleted", definition.IsDeleted.Value);

            return fixture.Create<ShoppingListStore>();
        }

        public IEnumerable<IShoppingListStore> CreateManyValid(int count = 3)
        {
            List<int> uniqueIds = commonFixture.NextUniqueInts(count).ToList();

            foreach (var id in uniqueIds)
            {
                var definition = ShoppingListStoreDefinition.FromId(id);

                yield return Create(definition);
            }
        }

        public IEnumerable<IShoppingListStore> CreateManyValid(ShoppingListStoreDefinition definition, int count = 3)
        {
            List<int> uniqueIds = commonFixture.NextUniqueInts(count).ToList();

            foreach (var id in uniqueIds)
            {
                var clone = definition.Clone();
                clone.Id = new ShoppingListStoreId(id);

                yield return Create(clone);
            }
        }

        public IEnumerable<IShoppingListStore> CreateManyValid(IEnumerable<ShoppingListStoreDefinition> definitions)
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
                definition.Id ??= new ShoppingListStoreId(id);
                yield return Create(definition);
            }
        }

        public IShoppingListStore CreateValid(ShoppingListStoreDefinition baseDefinition)
        {
            baseDefinition.IsDeleted ??= false;
            return Create(baseDefinition);
        }

        public IShoppingListStore CreateValid()
        {
            return CreateValid(new ShoppingListStoreDefinition());
        }
    }
}