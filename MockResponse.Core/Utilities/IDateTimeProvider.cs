using System;

namespace MockResponse.Core.Utilities
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}