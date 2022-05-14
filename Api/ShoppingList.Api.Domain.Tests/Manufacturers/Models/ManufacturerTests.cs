using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;
using ShoppingList.Api.Domain.TestKit.Common;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Models;

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
        public void Modify_WithModificationIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = _fixture.CreateSut();

            // Act
            var action = () => sut.Modify(null);

            // Assert

            action.Should().ThrowExactly<ArgumentNullException>().WithMessage("*modification*");
        }

        [Fact]
        public void Modify_WithValidData_ShouldThrowArgumentNullException()
        {
            // Arrange
            _fixture.SetupManufacturerModification();
            var sut = _fixture.CreateSut();
            var expected = new Manufacturer(sut.Id, _fixture.Modification.Name, sut.IsDeleted);

            // Act
            sut.Modify(_fixture.Modification);

            // Assert

            sut.Should().BeEquivalentTo(expected);
        }

        private sealed class ModifyFixture : LocalFixture
        {
            public ManufacturerModification Modification { get; private set; }

            public void SetupManufacturerModification()
            {
                Modification = new DomainTestBuilder<ManufacturerModification>().Create();
            }
        }
    }

    private abstract class LocalFixture
    {
        private readonly ManufacturerBuilder _manufacturerBuilder;

        protected LocalFixture()
        {
            _manufacturerBuilder = new ManufacturerBuilder();
        }

        public Manufacturer CreateSut()
        {
            return _manufacturerBuilder.Create();
        }
    }
}