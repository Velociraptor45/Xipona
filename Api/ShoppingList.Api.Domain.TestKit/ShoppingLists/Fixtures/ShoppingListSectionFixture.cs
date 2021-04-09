using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListSectionFixture : IModelFixture<IShoppingListSection, ShoppingListSectionDefinition>
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;

        public ShoppingListSectionFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
            this.shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
        }

        public IShoppingListSection Create(ShoppingListSectionDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<ShoppingListSection, SectionId>("id", definition.Id);
            if (definition.Name != null)
                fixture.ConstructorArgumentFor<IShoppingListSection, string>("name", definition.Name);
            if (definition.SortingIndex.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListSection, int>("sortingIndex", definition.SortingIndex.Value);
            if (definition.IsDefaultSection.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListSection, bool>("isDefaultSection", definition.IsDefaultSection.Value);
            if (definition.Items != null)
                fixture.ConstructorArgumentFor<ShoppingListSection, IEnumerable<IShoppingListItem>>("shoppingListItems", definition.Items);

            return fixture.Create<ShoppingListSection>();
        }

        public IEnumerable<IShoppingListSection> CreateMany(IEnumerable<ShoppingListSectionDefinition> definitions)
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
                    definition.Id = new SectionId(newIds.First());
                    newIds.RemoveAt(0);
                }

                yield return Create(definition);
            }
        }

        public IEnumerable<IShoppingListSection> CreateManyValid(int count = 3)
        {
            List<int> uniqueIds = commonFixture.NextUniqueInts(count).ToList();

            List<ShoppingListSectionDefinition> definitions = new List<ShoppingListSectionDefinition>();
            foreach (var id in uniqueIds)
            {
                var definition = ShoppingListSectionDefinition.FromId(id);
                definitions.Add(definition);
            }

            return CreateManyValid(definitions);
        }

        public IEnumerable<IShoppingListSection> CreateManyValid(ShoppingListSectionDefinition definition, int count = 3)
        {
            List<int> uniqueIds = commonFixture.NextUniqueInts(count).ToList();

            List<ShoppingListSectionDefinition> clonedDefinitions = new List<ShoppingListSectionDefinition>();
            foreach (var id in uniqueIds)
            {
                var clone = definition.Clone();
                clone.Id = new SectionId(id);
                clonedDefinitions.Add(clone);
            }

            return CreateManyValid(clonedDefinitions);
        }

        public IEnumerable<IShoppingListSection> CreateManyValid(IEnumerable<ShoppingListSectionDefinition> definitions)
        {
            var definitionsList = definitions.ToList();
            IEnumerable<int> existingIds = definitionsList
                .Where(d => d.Id != null)
                .Select(d => d.Id.Value);
            List<int> uniqueIds = commonFixture
                .NextUniqueInts(definitionsList.Count, existingIds)
                .ToList();

            EnsurePresenceOfDefaultSection(definitionsList);

            for (int i = 0; i < definitionsList.Count; i++)
            {
                int id = uniqueIds[i];
                var definition = definitionsList[i];
                definition.Id ??= new SectionId(id);
                yield return Create(definition);
            }
        }

        public IShoppingListSection CreateValid(ShoppingListSectionDefinition baseDefinition)
        {
            var itemCount = commonFixture.NextInt(3, 5);
            baseDefinition.Items ??= shoppingListItemFixture.CreateManyValid(itemCount).ToList();

            return Create(baseDefinition);
        }

        private void EnsurePresenceOfDefaultSection(IEnumerable<ShoppingListSectionDefinition> definitions)
        {
            var definitionsList = definitions.ToList();

            bool hasDefaultSection = definitionsList
                .Where(d => d.IsDefaultSection.HasValue)
                .Any(d => d.IsDefaultSection.Value);

            var definitionsWithoutDefaultSectionValue = definitionsList
                .Where(d => !d.IsDefaultSection.HasValue)
                .ToList();

            if (!definitionsWithoutDefaultSectionValue.Any())
                return;

            if (hasDefaultSection)
            {
                definitionsWithoutDefaultSectionValue.ForEach(d => d.IsDefaultSection = false);
            }
            else
            {
                var defaultSectionIndex = commonFixture.NextInt(0, definitionsWithoutDefaultSectionValue.Count - 1);

                for (int i = 0; i < definitionsWithoutDefaultSectionValue.Count; i++)
                {
                    definitionsWithoutDefaultSectionValue[i].IsDefaultSection = i == defaultSectionIndex;
                }
            }
        }
    }
}