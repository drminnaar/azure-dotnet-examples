
using System;

namespace TableStorage.CrudApi.Services.Dates;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
    public DateTime MaxValue => DateTime.MaxValue;
}
