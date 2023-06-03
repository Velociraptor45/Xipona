using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Stores.Models;

public class SectionTests
{
    public class Modify
    {
        private readonly ModifyFixture _fixture = new();

        [Fact]
        public void Modify_WithDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupSectionModification();
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionModification);

            // Act
            var func = () => sut.Modify(_fixture.SectionModification);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotModifyDeletedSection);
        }

        private sealed class ModifyFixture : SectionFixture
        {
            public SectionModification? SectionModification { get; private set; }

            public void SetupSectionModification()
            {
                SectionModification = new DomainTestBuilder<SectionModification>().Create();
            }
        }
    }

    private abstract class SectionFixture
    {
        private readonly SectionBuilder _sectionBuilder = new SectionBuilder();

        public void SetupDeleted()
        {
            _sectionBuilder.WithIsDeleted(true);
        }

        public Section CreateSut()
        {
            return _sectionBuilder.Create();
        }
    }
}