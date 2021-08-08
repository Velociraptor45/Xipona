using FsCheck;
using FsCheck.Xunit;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Api.Core.Tests.Experimental.Extensions
{
    public class ObjectExtensionsTests
    {
        [Property]
        public Property ToMonoList_IsLikeListWithOneItem(TestClass @class)
        {
            var comparer = new ObjectsComparer.Comparer<List<TestClass>>();
            return (comparer.Compare(@class.ToMonoList(), new List<TestClass> { @class })).ToProperty();
        }

        public class TestClass
        {
            public TestClass(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }
    }
}
