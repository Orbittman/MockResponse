using System;

namespace MockResponse.Core.Utilities
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}