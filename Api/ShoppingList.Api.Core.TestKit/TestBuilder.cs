using ShoppingList.Api.TestTools.AutoFixture;

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

    public new TBuilder FillPropertyWith<TParameter>(string propertyName, TParameter value)
    {
        this.PropertyFor<TModel, TParameter>(propertyName, value);
        return (TBuilder)this;
    }
}

public class TestBuilder<TModel> : TestBuilder<TModel, TestBuilder<TModel>>
{
}