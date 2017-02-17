using System;
using System.Collections.Concurrent;
using System.Net.Http;

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
        private readonly ConcurrentDictionary<string, ThrottleState> _hostCounts = new ConcurrentDictionary<string, ThrottleState>();

        public Throttler(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public void Throttle(string requestHost)
        {
            _hostCounts.AddOrUpdate(
                requestHost,
                s => new ThrottleState(_dateTimeProvider.Now),
                (s, state) =>
                {
                    if (state.ResetTime <= _dateTimeProvider.Now)
                    {
                        return new ThrottleState(_dateTimeProvider.Now);
                    }

                    if (state.Counter >= _throttleCount)
                    {
                        throw new HttpRequestException();
                    }

                    state.Counter++;
                    return state;
                });
        }

        private class ThrottleState
        {
            public ThrottleState(DateTime resetTime)
            {
                ResetTime = resetTime;
            }

            public int Counter { get; set; }

            public DateTime ResetTime { get; private set; }
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
