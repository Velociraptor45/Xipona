using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;

namespace ShoppingList.Api.Core.TestKit
{
    public class TestBuilderBase<TModel> : Fixture
    {
        public TestBuilderBase()
        {
            Customize(new AutoMoqCustomization { ConfigureMembers = true });
        }

        protected void FillContructorWith<TParameter>(string parameterName, TParameter value)
        {
            this.ConstructorArgumentFor<TModel, TParameter>(parameterName, value);
        }

        public TModel Create()
        {
            return this.Create<TModel>();
        }
    }
}