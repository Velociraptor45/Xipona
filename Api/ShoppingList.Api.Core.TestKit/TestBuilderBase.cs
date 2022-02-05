using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;

namespace ShoppingList.Api.Core.TestKit;

public class TestBuilderBase<TModel> : Fixture
{
    public TestBuilderBase()
    {
        Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }

    protected void FillConstructorWith<TParameter>(string parameterName, TParameter value)
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