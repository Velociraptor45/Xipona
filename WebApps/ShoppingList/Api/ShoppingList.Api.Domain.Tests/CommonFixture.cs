using AutoFixture;
using AutoFixture.AutoMoq;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests
{
    public static class CommonFixture
    {
        private static readonly Random random = new Random();

        public static Fixture GetNewFixture()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            return fixture;
        }

        public static int NextInt()
        {
            return random.Next(1, int.MaxValue);
        }
    }
}