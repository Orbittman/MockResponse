using System;

namespace MockResponse.Api.Queries
{
    public class ResponseParameters : IPageable
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}