using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Manufacturers.Models;

public class ManufacturerTests
{
    public class Modify
    {
        private readonly ModifyFixture _fixture;

        public Modify()
        {
            _fixture = new ModifyFixture();
        }

        [Fact]
        public void Modify_WithDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupManufacturerModification();
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var func = () => sut.Modify(_fixture.Modification);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotModifyDeletedManufacturer);
        }

        [Fact]
        public void Modify_WithValidData_ShouldModifyManufacturer()
        {
            // Arrange
            _fixture.SetupManufacturerModification();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            var expected = new Manufacturer(sut.Id, _fixture.Modification.Name, sut.IsDeleted);

            // Act
            sut.Modify(_fixture.Modification);

            // Assert
            sut.Should().BeEquivalentTo(expected);
        }

        private sealed class ModifyFixture : ManufacturerFixture
        {
            public ManufacturerModification? Modification { get; private set; }

            public void SetupManufacturerModification()
            {
                Modification = new DomainTestBuilder<ManufacturerModification>().Create();
            }
        }
    }

    public class Delete
    {
        private readonly DeleteFixture _fixture;

        public Delete()
        {
            _fixture = new DeleteFixture();
        }

        [Fact]
        public void Delete_WithNotDeleted_ShouldDeleteManufacturer()
        {
            // Arrange
            _fixture.SetupManufacturerNotDeleted();
            var sut = _fixture.CreateSut();

            // Act
            sut.Delete();

            // Assert

            sut.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public void Delete_WithDeleted_ShouldNotChangeDeletionOfManufacturer()
        {
            // Arrange
            _fixture.SetupManufacturerDeleted();
            var sut = _fixture.CreateSut();

            // Act
            sut.Delete();

            // Assert

            sut.IsDeleted.Should().BeTrue();
        }

        private sealed class DeleteFixture : ManufacturerFixture
        {
            public void SetupManufacturerNotDeleted()
            {
                ManufacturerBuilder.WithIsDeleted(false);
            }

            public void SetupManufacturerDeleted()
            {
                ManufacturerBuilder.WithIsDeleted(true);
            }
        }
    }

    private abstract class ManufacturerFixture
    {
        protected readonly ManufacturerBuilder ManufacturerBuilder;

        protected ManufacturerFixture()
        {
            ManufacturerBuilder = new ManufacturerBuilder();
            ManufacturerBuilder.WithIsDeleted(false);
        }

        public void SetupDeleted()
        {
            ManufacturerBuilder.WithIsDeleted(true);
        }

        public Manufacturer CreateSut()
        {
            return ManufacturerBuilder.Create();
        }
    }
}