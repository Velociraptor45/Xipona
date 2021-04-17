using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.Stores.Fixtures
{
    public class StoreFixture
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreSectionFixture storeSectionFixture;

        public StoreFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
            storeSectionFixture = new StoreSectionFixture(commonFixture);
        }

        public IStore GetStore(StoreId id = null, bool? isDeleted = null)
        {
            var fixture = commonFixture.GetNewFixture();

            if (id != null)
                fixture.ConstructorArgumentFor<Store, StoreId>("id", id);
            if (isDeleted.HasValue)
                fixture.ConstructorArgumentFor<Store, bool>("isDeleted", isDeleted.Value);

            return fixture.Create<Store>();
        }

        public IEnumerable<IStore> GetStores(int count = 3, bool? isDeleted = null)
        {
            if (count < 1)
                throw new ArgumentException($"{nameof(count)} mustn't be smaller than 1.");

            var uniqueStoreIds = commonFixture.NextUniqueInts(count);

            foreach (int uniqueStoreId in uniqueStoreIds)
            {
                yield return GetStore(new StoreId(uniqueStoreId), isDeleted);
            }
        }

        public IStore Create(StoreDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<Store, StoreId>("id", definition.Id);
            if (definition.Name != null)
                fixture.ConstructorArgumentFor<Store, string>("name", definition.Name);
            if (definition.IsDeleted != null)
                fixture.ConstructorArgumentFor<Store, bool>("isDeleted", definition.IsDeleted.Value);
            if (definition.Sections != null)
                fixture.ConstructorArgumentFor<Store, IEnumerable<IStoreSection>>("sections", definition.Sections);

            return fixture.Create<Store>();
        }

        public IEnumerable<IStore> Create(IEnumerable<StoreDefinition> definitions)
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
                    definition.Id = new StoreId(newIds.First());
                    newIds.RemoveAt(0);
                }

                yield return Create(definition);
            }
        }

        public IStore CreateValid()
        {
            return CreateValid(new StoreDefinition());
        }

        public IStore CreateValid(StoreDefinition baseDefinition)
        {
            baseDefinition.Sections ??= CreateValidSections(3).ToList();
            baseDefinition.IsDeleted ??= false;

            return Create(baseDefinition);
        }

        public IStore CreateValid(StoreDefinition baseDefinition, int sectionAmount)
        {
            List<IStoreSection> sections = CreateValidSections(sectionAmount).ToList();

            baseDefinition.Sections ??= sections;
            baseDefinition.IsDeleted ??= false;

            return Create(baseDefinition);
        }

        public IEnumerable<IStore> CreateManyValid(int count = 3)
        {
            return CreateManyValid(new StoreDefinition(), count);
        }

        public IEnumerable<IStore> CreateManyValid(IEnumerable<StoreDefinition> definitions)
        {
            var definitionsList = definitions.ToList();

            var uniqueStoreIds = commonFixture.NextUniqueInts(definitionsList.Count).ToList();

            for (int i = 0; i < definitionsList.Count; i++)
            {
                StoreDefinition definition = definitionsList[i];
                definition.Id ??= new StoreId(uniqueStoreIds[i]);
                yield return CreateValid(definition);
            }
        }

        public IEnumerable<IStore> CreateManyValid(StoreDefinition baseDefinition, int count = 3)
        {
            if (count <= 0)
                throw new ArgumentException($"{nameof(count)} must be greater than 0.");

            var uniqueStoreIds = commonFixture.NextUniqueInts(count);

            foreach (int uniqueStoreId in uniqueStoreIds)
            {
                var definition = baseDefinition.Clone();
                definition.Id = new StoreId(uniqueStoreId);
                yield return Create(definition);
            }
        }

        // todo: outsource this to StoreSectionFixture
        private IEnumerable<IStoreSection> CreateValidSections(int sectionAmount)
        {
            List<IStoreSection> sections = storeSectionFixture
                .CreateMany(StoreSectionDefinition.FromIsDefaultSection(false), sectionAmount - 1)
                .ToList();
            IEnumerable<int> otherSectionIds = sections.Select(s => s.Id.Value);

            int defaultSectionId = commonFixture.NextUniqueInts(1, otherSectionIds).First();
            var defaultSectionDefinition = new StoreSectionDefinition()
            {
                Id = new SectionId(defaultSectionId),
                IsDefaultSection = true
            };
            IStoreSection defaultSection = storeSectionFixture.Create(defaultSectionDefinition);

            sections.Add(defaultSection);
            sections.Shuffle();

            return sections;
        }
    }
}