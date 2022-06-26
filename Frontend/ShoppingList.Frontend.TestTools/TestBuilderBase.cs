using AutoFixture;
using AutoFixture.AutoMoq;
using ShoppingList.Frontend.TestTools.AutoFixture;

namespace ShoppingList.Frontend.TestTools;

public abstract class TestBuilderBase<TModel> : Fixture
{
    protected TestBuilderBase()
    {
        Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }

    protected TestBuilderBase(ICustomization customization) : this()
    {
        customization.Customize(this);
    }

    public void FillConstructorWith<TParameter>(string parameterName, TParameter value)
    {
        this.ConstructorArgumentFor<TModel, TParameter>(parameterName, value);
    }

    public TModel Create()
    {
        return this.Create<TModel>();
    }

    public IEnumerable<TModel> CreateMany(int count)
    {
        return this.CreateMany<TModel>(count);
    }
}