using AutoFixture.Kernel;
using System.Linq;

namespace ShoppingList.Api.Core.TestKit;

public class EnumExclusionCustomization<T> : ICustomization
    where T : Enum
{
    private readonly IEnumerable<T> _exclude;

    public EnumExclusionCustomization(IEnumerable<T> exclude)
    {
        _exclude = exclude;
    }

    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Insert(0, new EnumExclusionSpecimenBuilder<T>(_exclude));
    }

    private class EnumExclusionSpecimenBuilder<TEnum> : ISpecimenBuilder
        where TEnum : Enum
    {
        private readonly List<string> _exclude;
        private readonly EnumGenerator _enumGenerator;

        public EnumExclusionSpecimenBuilder(IEnumerable<TEnum> exclude)
        {
            _exclude = exclude.Select(e => e.ToString()).ToList();
            _enumGenerator = new EnumGenerator();
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            for (int i = 0; i < 20; i++)
            {
                var specimen = _enumGenerator.Create(request, context);
                if (!_exclude.Contains(specimen.ToString()))
                {
                    return specimen;
                }
            }

            throw new ObjectCreationException($"Couldn't create object of type {typeof(TEnum).Name} after 20 attempts");
        }

        private bool MatchesType(object request)
        {
            var t = request as Type;
            return typeof(TEnum) == t;
        }
    }
}