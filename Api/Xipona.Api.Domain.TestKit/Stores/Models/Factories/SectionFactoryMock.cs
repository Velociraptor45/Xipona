using ProjectHermes.Xipona.Api.Domain.Stores.Models.Factories;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Models.Factories;

public class SectionFactoryMock : Mock<ISectionFactory>
{
    public SectionFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }
}