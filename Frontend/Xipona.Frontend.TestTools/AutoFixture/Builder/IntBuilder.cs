using ProjectHermes.Xipona.Frontend.TestTools.AutoFixture.Customizations;

namespace ProjectHermes.Xipona.Frontend.TestTools.AutoFixture.Builder;

public class IntBuilder : TestBuilderBase<int>
{
    public int CreatePositive(int minValue = 1, int maxValue = int.MaxValue)
    {
        Customize(new PositiveIntCustomization(minValue, maxValue));
        return Create();
    }

    public int CreateNegative(int minValue = int.MinValue)
    {
        Customize(new NegativeIntCustomization(minValue));
        return Create();
    }
}