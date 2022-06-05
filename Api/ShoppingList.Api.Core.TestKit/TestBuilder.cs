namespace ShoppingList.Api.Core.TestKit;

public class TestBuilder<TModel, TBuilder> : TestBuilderBase<TModel>
    where TBuilder : TestBuilder<TModel, TBuilder>
{
    public TestBuilder()
    {
    }

    public TestBuilder(ICustomization customization) : base(customization)
    {
    }

    public new TBuilder FillConstructorWith<TParameter>(string parameterName, TParameter value)
    {
        base.FillConstructorWith(parameterName, value);
        return (TBuilder)this;
    }
}

public class TestBuilder<TModel> : TestBuilder<TModel, TestBuilder<TModel>>
{
}