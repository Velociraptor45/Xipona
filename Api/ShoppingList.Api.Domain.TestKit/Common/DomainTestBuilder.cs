namespace ShoppingList.Api.Domain.TestKit.Common;

public class DomainTestBuilder<TModel> : DomainTestBuilderBase<TModel>
{
    public DomainTestBuilder() : base()
    {
    }

    public new DomainTestBuilder<TModel> FillConstructorWith<TParameter>(string parameterName, TParameter value)
    {
        base.FillConstructorWith(parameterName, value);
        return this;
    }
}