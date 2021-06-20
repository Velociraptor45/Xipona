using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models
{
    public class ShoppingListSectionFactoryMock
    {
        private readonly Mock<IShoppingListSectionFactory> mock;

        public ShoppingListSectionFactoryMock(Mock<IShoppingListSectionFactory> mock)
        {
            this.mock = mock;
        }

        public ShoppingListSectionFactoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IShoppingListSectionFactory>>();
        }

        public void SetupCreateEmpty(SectionId sectionId, IShoppingListSection returnValue)
        {
            mock
                .Setup(instance => instance.CreateEmpty(
                    It.Is<SectionId>(id => id == sectionId)))
                .Returns(returnValue);
        }

        public void VerifyCreateEmptyOnce(SectionId sectionId)
        {
            mock
                .Verify(i => i.CreateEmpty(
                        It.Is<SectionId>(id => id == sectionId)),
                    Times.Once);
        }
    }
}