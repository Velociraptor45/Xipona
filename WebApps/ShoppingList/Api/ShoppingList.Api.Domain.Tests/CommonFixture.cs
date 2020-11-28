using AutoFixture;
using AutoFixture.AutoMoq;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public int NextInt(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public int NextInt()
        {
            return random.Next(1, int.MaxValue);
        }

        public int NextInt(IEnumerable<int> exclude)
        {
            List<int> excludedInts = exclude.ToList();
            while (true)
            {
                var number = random.Next(1, int.MaxValue);
                if (!excludedInts.Contains(number))
                    return number;
            }
        }

        public int NextInt(int exclude)
        {
            return NextInt(new List<int> { exclude });
        }

        public bool NextBool()
        {
            return random.NextDouble() < .5f;
        }

        public float NextFloat()
        {
            return (float)random.NextDouble();
        }
    }
}