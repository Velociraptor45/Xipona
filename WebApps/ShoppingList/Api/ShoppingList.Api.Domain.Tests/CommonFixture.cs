using AutoFixture;
using AutoFixture.AutoMoq;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests
{
    public class CommonFixture
    {
        private readonly Random random = new Random();

        public Fixture GetNewFixture()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            return fixture;
        }

        public int NextInt()
        {
            return random.Next(1, int.MaxValue);
        }
    }
}