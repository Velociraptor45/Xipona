using AutoFixture;
using AutoFixture.AutoMoq;
using Xipona.Frontend.TestTools.AutoFixture;

namespace Xipona.Frontend.TestTools;

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

    public virtual TModel Create()
    {
        return this.Create<TModel>();
    }

    public IEnumerable<TModel> CreateMany(int count)
    {
        return this.CreateMany<TModel>(count);
    }
}