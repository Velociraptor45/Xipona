using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools;
using ProjectHermes.ShoppingList.Frontend.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Common;

public abstract class ToDomainConverterBase<TSource, TDest, TConverter> where TConverter : IToDomainConverter<TSource, TDest>
{
    protected bool ExcludingKey { get; set; } = true;

    [Fact]
    public void Convert_ShouldMapAllMembersCorrectly()
    {
        // Arrange
        var contract = new TestBuilder<TSource>().Create();
        var mapper = new MapperConfiguration(cfg =>
        {
            AddMapping(cfg);
            AddAdditionalMapping(cfg);
        }).CreateMapper();

        mapper.ConfigurationProvider.AssertConfigurationIsValid();

        var expectedResult = mapper.Map<TDest>(contract);
        var sut = CreateSut();

        // Act
        var result = sut.ToDomain(contract);

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt =>
        {
            if (ExcludingKey)
                opt.ExcludingKey();

            return opt;
        });
    }

    protected abstract TConverter CreateSut();

    protected virtual void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
    }

    protected abstract void AddMapping(IMappingExpression<TSource, TDest> mapping);

    public void AddMapping(IMapperConfigurationExpression cfg)
    {
        var mapping = cfg.CreateMap<TSource, TDest>(MemberList.Destination);
        AddMapping(mapping);
    }
}