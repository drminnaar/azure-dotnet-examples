using System;
using Azure;
using Azure.Data.Tables;

namespace FakeData.Weather
{
    public sealed record ForecastTableEntity : Forecast, ITableEntity
    {
        public ForecastTableEntity() : base()
        {
        }

        public ForecastTableEntity(Forecast forecast)
        {
            PartitionKey = forecast.City;
            RowKey = forecast.ThermometerId;
            ThermometerId = forecast.ThermometerId;
            ThermometerName = forecast.ThermometerName;
            Date = forecast.Date;
            Time = forecast.Time;
            Low = forecast.Low;
            High = forecast.High;
            Conditions = forecast.Conditions;
            City = forecast.City;
        }

        public string PartitionKey { get; set; } = string.Empty;
        public string RowKey { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
