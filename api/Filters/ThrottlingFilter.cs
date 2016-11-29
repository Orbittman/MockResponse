using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Filters
{
    public class ThrottlingFilter : ActionFilterAttribute
    {
        private readonly IThrottler _throttler;

        public ThrottlingFilter(IThrottler throttler)
        {
            _throttler = throttler;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _throttler.Throttle(context.HttpContext.Request.Host.Host);
            base.OnActionExecuting(context);
        }
    }

    public interface IThrottler
    {
        void Throttle(string requestHost);
    }

    public class Throttler : IThrottler
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private const int _throttleWindowSeconds = 10;
        private const int _throttleCount = 10;
        private ConcurrentDictionary<string, int> _hostCounts = new ConcurrentDictionary<string, int>();

        public Throttler(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public void Throttle(string requestHost)
        {
            
        }
    }

    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
