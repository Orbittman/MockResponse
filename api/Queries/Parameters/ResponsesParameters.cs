using MockResponse.Core.Data.Models;

namespace MockResponse.Api.Queries.Parameters
{
    public class ResponsesParameters : IPageable
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}