using FsCheck;
using FsCheck.Xunit;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.Tests.Experimental.TestKit.StoreItems.Models.StoreItems;
using System;
using System.Linq;

namespace ShoppingList.Api.Domain.Tests.Experimental.StoreItems.Models.StoreItems
{
    public class StoreItemTests
    {
        [Property(Arbitrary = new[] { typeof(StoreItemArbitrary) })]
        public Property GetDefaultSectionIdForStore_WithInvalidStoreId_ShouldThrowDomainException2(StoreItem item,
            int requestStoreId)
        {
            // Arrange
            var allStoreIds = item.Availabilities.Select(av => av.StoreId.Value);

            // Act
            Func<SectionId> func = () => item.GetDefaultSectionIdForStore(new StoreId(requestStoreId));

            // Assert
            return Prop.Throws<DomainException, SectionId>(new Lazy<SectionId>(func))
                .When(!allStoreIds.Contains(requestStoreId));
        }

        [Property(Arbitrary = new[] { typeof(StoreItemArbitrary) })]
        public Property SetPredecessor_WithPredecessor_ShouldSetPredecessor(StoreItem item, StoreItem predecessor)
        {
            item.SetPredecessor(predecessor);
            return (item.Predecessor == predecessor).ToProperty();
        }
    }
}