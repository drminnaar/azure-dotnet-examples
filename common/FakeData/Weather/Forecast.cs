using System;

namespace FakeData.Weather
{
    public record Forecast : FakeEntityBase<Forecast>
    {
        public Forecast() : base()
        {
        }

        public string ThermometerId { get; init; } = string.Empty;
        public string ThermometerName { get; init; } = string.Empty;
        public string Date { get; init; } = string.Empty;
        public string Time { get; init; } = string.Empty;
        public double Low { get; init; }
        public double High { get; init; }
        public string Conditions { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;

        public override int GetConsistentHashCode() => ThermometerId.GetConsistentHashCode();
    }
}
