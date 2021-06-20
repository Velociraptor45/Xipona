using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.Stores.Fixtures
{
    public class StoreSectionFixture
    {
        private readonly CommonFixture commonFixture;

        public StoreSectionFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public IStoreSection Create(StoreSectionDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<StoreSection, SectionId>("id", definition.Id);
            if (definition.Name != null)
                fixture.ConstructorArgumentFor<StoreSection, string>("name", definition.Name);
            if (definition.SortingIndex != null)
                fixture.ConstructorArgumentFor<StoreSection, int>("sortingIndex", definition.SortingIndex.Value);
            if (definition.IsDefaultSection != null)
                fixture.ConstructorArgumentFor<StoreSection, bool>("isDefaultSection", definition.IsDefaultSection.Value);

            return fixture.Create<StoreSection>();
        }

        public IEnumerable<IStoreSection> Create(IEnumerable<StoreSectionDefinition> definitions)
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

        public IEnumerable<IStoreSection> CreateMany(StoreSectionDefinition definition, int amount)
        {
            var definitions = new List<StoreSectionDefinition>();
            for (int i = 0; i < amount; i++)
            {
                definitions.Add(definition.Clone());
            }

            return Create(definitions);
        }
    }
}