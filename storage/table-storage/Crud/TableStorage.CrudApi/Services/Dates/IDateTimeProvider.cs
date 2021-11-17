using System;

namespace TableStorage.CrudApi.Services.Dates
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now { get; }
        DateTime MaxValue { get; }
    }
}
