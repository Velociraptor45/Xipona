using AutoFixture.Kernel;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.SpecimenBuilders;

public class EnumSpecimenBuilder<TEnum> : ISpecimenBuilder
    where TEnum : Enum
{
    private readonly CommonFixture _commonFixture = new();

    public object Create(object request, ISpecimenContext context)
    {
        if (!(request is TEnum))
        {
            return new NoSpecimen();
        }

        var values = ((TEnum[])Enum.GetValues(typeof(TEnum)))
            .ToList();

        return _commonFixture.ChooseRandom(values);
    }
}