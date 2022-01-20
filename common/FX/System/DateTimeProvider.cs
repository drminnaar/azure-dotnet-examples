namespace System;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
    DateTime MaxValue { get; }
    DateTime MinValue { get; }
}

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
    public DateTime MaxValue => DateTime.MaxValue;
    public DateTime MinValue => DateTime.MinValue;
}
