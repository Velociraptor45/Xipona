using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListSectionFixture
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;

        public ShoppingListSectionFixture(CommonFixture commonFixture, ShoppingListItemFixture shoppingListItemFixture)
        {
            this.commonFixture = commonFixture;
            this.shoppingListItemFixture = shoppingListItemFixture;
        }

        public IShoppingListSection Create()
        {
            int sectionId = commonFixture.NextInt();
            IEnumerable<int> itemIds = commonFixture.NextUniqueInts(3);
            IEnumerable<ShoppingListItemId> shoppingListItemIds = itemIds.Select(id => new ShoppingListItemId(id));
            return Create(new ShoppingListSectionId(sectionId), shoppingListItemIds);
        }

        public IShoppingListSection Create(ShoppingListSectionId id)
        {
            var items = shoppingListItemFixture.CreateMany(3);
            return Create(id, items);
        }

        public IShoppingListSection Create(ShoppingListSectionId id, IEnumerable<ShoppingListItemId> itemIds)
        {
            var items = shoppingListItemFixture.CreateMany(itemIds);
            return Create(id, items);
        }

        public IShoppingListSection Create(ShoppingListSectionId id, IEnumerable<IShoppingListItem> items)
        {
            var definition = new ShoppingListSectionGenerationDefinition
            {
                Id = id,
                Items = items
            };

            return Create(definition);
        }

        public IShoppingListSection Create(ShoppingListSectionGenerationDefinition configuration)
        {
            var fixture = commonFixture.GetNewFixture();

            if (configuration.Id != null)
                fixture.ConstructorArgumentFor<ShoppingListSection, ShoppingListSectionId>("id", configuration.Id);
            if (configuration.Name != null)
                fixture.ConstructorArgumentFor<IShoppingListSection, string>("name", configuration.Name);
            if (configuration.SortingIndex.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListSection, int>("sortingIndex", configuration.SortingIndex.Value);
            if (configuration.IsDefaultSection.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListSection, bool>("isDefaultSection", configuration.IsDefaultSection.Value);
            if (configuration.Items != null)
                fixture.ConstructorArgumentFor<ShoppingListSection, IEnumerable<IShoppingListItem>>("shoppingListItems", configuration.Items);

            return fixture.Create<ShoppingListSection>();
        }

        public IEnumerable<IShoppingListSection> CreateMany(IEnumerable<SectionIdMapping> mappings)
        {
            foreach (var mapping in mappings)
            {
                yield return Create(mapping.SectionId, mapping.ItemIds);
            }
        }

        public IEnumerable<IShoppingListSection> CreateMany(int count)
        {
            IEnumerable<int> uniqueRawSectionIds = commonFixture.NextUniqueInts(count);
            List<int> uniqueRawItemIds = commonFixture.NextUniqueInts(count * 3).ToList();
            List<ShoppingListSectionId> uniqueSectionIds = uniqueRawSectionIds.Select(id => new ShoppingListSectionId(id)).ToList();
            List<ShoppingListItemId> uniqueItemIds = uniqueRawItemIds.Select(id => new ShoppingListItemId(id)).ToList();

            List<SectionIdMapping> mappings = new List<SectionIdMapping>();
            for (int i = 0; i < uniqueSectionIds.Count; i++)
            {
                var sectionId = uniqueSectionIds[i];
                var itemIds = uniqueItemIds.GetRange(i * 3, 3);
                var mapping = new SectionIdMapping(sectionId, itemIds);
                mappings.Add(mapping);
            }

            return CreateMany(mappings);
        }

        public IEnumerable<IShoppingListSection> CreateMany(IEnumerable<ShoppingListSectionGenerationDefinition> definitions)
        {
            var existingIds = definitions
                .Where(def => def.Id != null)
                .Select(d => d.Id.Value);
            var newIdCount = definitions.ToList().Count - existingIds.ToList().Count;
            var newIds = commonFixture.NextUniqueInts(newIdCount, exclude: existingIds).ToList();

            foreach (var definition in definitions)
            {
                if (definition.Id == null)
                {
                    definition.Id = new ShoppingListSectionId(newIds.First());
                    newIds.RemoveAt(0);
                }

                yield return Create(definition);
            }
        }

        public IEnumerable<IShoppingListSection> CreateSpecialAndRandom(ShoppingListSectionGenerationDefinition definition, int randomCount = 3)
        {
            IShoppingListSection special = Create(definition);
            IEnumerable<IShoppingListSection> randoms = CreateMany(randomCount);

            var result = randoms.ToList();
            result.Add(special);
            result.Shuffle();
            return result;
        }

        public class SectionIdMapping
        {
            private readonly IEnumerable<ShoppingListItemId> itemIds;

            public SectionIdMapping(ShoppingListSectionId sectionId, IEnumerable<ShoppingListItemId> itemIds)
            {
                SectionId = sectionId;
                this.itemIds = itemIds;
            }

            public ShoppingListSectionId SectionId { get; }
            public IReadOnlyCollection<ShoppingListItemId> ItemIds => itemIds.ToList().AsReadOnly();
        }
    }
}