using Bogus;
using System;
using System.Collections.Generic;

namespace FakeData
{
    public abstract class FakeEntityGeneratorBase<T> where T : class, new()
    {
        static FakeEntityGeneratorBase()
        {
            Randomizer.Seed = new Random(8675309);
        }

        protected FakeEntityGeneratorBase()
        {
        }

        public abstract IReadOnlyCollection<T> GenerateFakes(int fakeCount);
    }
}
