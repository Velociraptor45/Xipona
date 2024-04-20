using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Items.Models;

public class ItemAvailabilityTests
{
    public class TransferToDefaultSection
    {
        private readonly TransferToDefaultSectionFixture _fixture = new();

        [Fact]
        public void TransferToDefaultSection_WithSectionIdNull_ShouldThrow()
        {
            // Arrange
            _fixture.SetupSectionId();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.TransferToDefaultSection(_fixture.SectionId.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class TransferToDefaultSectionFixture : ItemAvailabilityFixture
        {
            public SectionId? SectionId { get; private set; }
            public ItemAvailability? ExpectedResult { get; private set; }

            public void SetupSectionId()
            {
                SectionId = Domain.Stores.Models.SectionId.New;
            }

            public void SetupExpectedResult(ItemAvailability sut)
            {
                TestPropertyNotSetException.ThrowIfNull(SectionId);

                ExpectedResult = new ItemAvailabilityBuilder()
                    .WithStoreId(sut.StoreId)
                    .WithPrice(sut.Price)
                    .WithDefaultSectionId(SectionId.Value)
                    .Create();
            }
        }
    }

    private abstract class ItemAvailabilityFixture
    {
        private readonly ItemAvailabilityBuilder _builder = new();

        public ItemAvailability CreateSut()
        {
            return _builder.Create();
        }
    }
}