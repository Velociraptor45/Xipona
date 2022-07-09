﻿using AutoFixture.AutoMoq;
using ProjectHermes.ShoppingList.Api.TestTools.AutoFixture;

namespace ProjectHermes.ShoppingList.Api.Core.TestKit;

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

    public void FillPropertyWith<TParameter>(string propertyName, TParameter value)
    {
        this.PropertyFor<TModel, TParameter>(propertyName, value);
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