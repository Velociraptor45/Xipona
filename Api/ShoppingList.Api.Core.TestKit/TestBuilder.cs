﻿using ShoppingList.Api.TestTools.AutoFixture;

namespace ShoppingList.Api.Core.TestKit;

public class TestBuilder<TModel, TBuilder> : TestBuilderBase<TModel>
    where TBuilder : TestBuilder<TModel, TBuilder>
{
    private readonly List<Action<TModel>> _postCreationActions = new();

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

    /// <summary>
    /// This method will fill a property matching the given propertyName with the provided value <b>upon</b> object
    /// creation if the data types match
    /// </summary>
    public new TBuilder FillPropertyWith<TParameter>(string propertyName, TParameter value)
    {
        this.PropertyFor<TModel, TParameter>(propertyName, value);
        return (TBuilder)this;
    }

    /// <summary>
    /// The provided action will execute on the created object <b>after</b> object creation.
    /// Thus, the object is created with random values at first. If you need to set properties upon creation because
    /// it will fail otherwise, use the <see cref="FillPropertyWith{TParameter}" /> method.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public TBuilder AfterCreation(Action<TModel> action)
    {
        _postCreationActions.Add(action);
        return (TBuilder)this;
    }

    public override TModel Create()
    {
        var obj = base.Create();

        _postCreationActions.ForEach(act => act(obj));

        return obj;
    }
}

public class TestBuilder<TModel> : TestBuilder<TModel, TestBuilder<TModel>>
{
}