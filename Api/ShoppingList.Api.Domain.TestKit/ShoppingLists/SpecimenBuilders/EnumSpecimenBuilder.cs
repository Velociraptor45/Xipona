using AutoFixture.Kernel;
using ShoppingList.Api.Domain.TestKit.Shared;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.SpecimenBuilders;

public class EnumSpecimenBuilder<TEnum> : ISpecimenBuilder
    where TEnum : Enum
{
    private readonly CommonFixture commonFixture;

    public EnumSpecimenBuilder()
    {
        commonFixture = new CommonFixture();
    }

    public object Create(object request, ISpecimenContext context)
    {
        if (!(request is TEnum))
        {
            return new NoSpecimen();
        }

        var values = ((TEnum[])Enum.GetValues(typeof(TEnum)))
            .ToList();

        return commonFixture.ChooseRandom(values);
    }
}