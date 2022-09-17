using AutoFixture.AutoMoq;
using ProjectHermes.ShoppingList.Api.TestTools.AutoFixture;
using System.Linq.Expressions;

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

    /// <summary>
    /// This method will fill a property matching the given propertyName with the provided value <b>upon</b> object
    /// creation if the data types match
    /// </summary>
    public void FillPropertyWith<TParameter>(Expression<Func<TModel, TParameter>> property, TParameter value)
    {
        // expression source: https://handcraftsman.wordpress.com/2008/11/11/how-to-get-c-property-names-without-magic-strings/
        var body = (MemberExpression)property.Body;
        this.PropertyFor<TModel, TParameter>(body.Member.Name, value);
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