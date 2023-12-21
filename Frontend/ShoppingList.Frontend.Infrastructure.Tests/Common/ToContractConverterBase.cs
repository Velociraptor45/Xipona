using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Common;

public abstract class ToContractConverterBase<TSource, TDest, TConverter> where TConverter : IToContractConverter<TSource, TDest>
{
    [Fact]
    public void Convert_ShouldMapAllMembersCorrectly()
    {
        // Arrange
        var contract = CreateSource();

        var mapper = new MapperConfiguration(AddMapping).CreateMapper();

        mapper.ConfigurationProvider.AssertConfigurationIsValid();

        var expectedResult = mapper.Map<TDest>(contract);
        var sut = CreateSut();

        // Act
        var result = sut.ToContract(contract);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    protected abstract TConverter CreateSut();

    protected virtual void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
    }

    protected abstract void AddMapping(IMappingExpression<TSource, TDest> mapping);

    protected virtual void Customize(IFixture fixture)
    {
    }

    protected virtual TSource CreateSource()
    {
        var builder = new TestBuilder<TSource>();
        Customize(builder);
        return builder.Create();
    }

    public void AddMapping(IMapperConfigurationExpression cfg)
    {
        var mapping = cfg.CreateMap<TSource, TDest>(MemberList.Destination);
        AddMapping(mapping);
        AddAdditionalMapping(cfg);
    }
}