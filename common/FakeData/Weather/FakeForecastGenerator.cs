using System.Collections.Generic;
using Bogus;

namespace FakeData.Weather
{
    public sealed class FakeForecastGenerator : FakeEntityGeneratorBase<Forecast>
    {
        private readonly IList<string> _conditions = new List<string>
        {
            "Sunny",
            "Cloudy",
            "Partly Cloudy"
        };

        private readonly IList<string> _cities = new List<string>
        {
            "Amsterdam",
            "Auckland",
            "Cape Town",
            "London",
            "New York",
            "Paris",
            "Sydney"
        };

        public override IReadOnlyCollection<Forecast> GenerateFakes(int fakeCount) =>
            new Faker<Forecast>()
                .RuleFor(fc => fc.City, (f, fc) => f.PickRandom(_cities))
                .RuleFor(fc => fc.Conditions, (f, fc) => f.PickRandom(_conditions))
                .RuleFor(fc => fc.Date, (f, fc) => f.Date.RecentOffset(days: 7).ToString("yyyy-MMM-dd"))
                .RuleFor(fc => fc.High, (f, fc) => f.Random.Double(min: 15, max: 30))
                .RuleFor(fc => fc.Low, (f, fc) => f.Random.Double(min: 5, max: 15))
                .RuleFor(fc => fc.ThermometerId, (f, fc) => f.Random.Hash())
                .RuleFor(fc => fc.ThermometerName, (f, fc) => $"{f.Hacker.Verb()}-{f.Hacker.Noun()}")
                .RuleFor(fc => fc.Time, (f, fc) => f.Date.Recent(days: 7).ToString("HH:mm:ss"))
                .Generate(fakeCount);
    }
}
